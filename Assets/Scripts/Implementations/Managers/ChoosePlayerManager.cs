using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePlayerManager : MonoBehaviour, IPlayersSelectorManager
{

    private static ChoosePlayerManager _instance = null;
    public static ChoosePlayerManager Instance
    {
        get
        {
            if (_instance == null) throw new System.NullReferenceException("Trying to get the Gamepad Manager, but it's not attached as a component!");
            return _instance;
        }
    }

    [System.Serializable]
    public class ColorMatch
    {
        public int colorId;
        public GameObject prefab;
        public Constants.BirdColor birdColor;
        public bool pointed = false;
        public override bool Equals(object obj)
        {
            if (obj is ColorMatch)
            {
                ColorMatch cm = (ColorMatch)obj;
                return (cm.colorId == this.colorId && cm.pointed == this.pointed);
            }
            return false;
        }
    }

    [System.Serializable]
    public class PlayerSelectorDto
    {
        public int PlayerNumber;
        public int ControllerId;
        public int ColorIndex;
        public Constants.BirdColor Color;
    }

    public List<PlayerSelectorDto> playerDtos;
    public List<ColorMatch> colorMatches;
    public List<GameObject> playerDisplayers;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomOpens();
    }

    public ColorMatch GetFirstFreeColor()
    {
        List<ColorMatch> actives = colorMatches.FindAll(c => c.pointed == false);
        if (actives.IsEmpty()) return null;
        actives = actives.OrderBy(c => c.colorId).ToList();
        return actives[0];
    }
    public ColorMatch GetNextFreeColor(ColorMatch actual)
    {
        ColorMatch toReturn = null;
        int toReturnIndex = -1;
        for (int i = 0, actualIndex = (actual == null) ? -1 : colorMatches.IndexOf(actual); i < colorMatches.Count; i++)
        {
            if (colorMatches[i].pointed || i == actualIndex) continue;
            if (toReturn != null && i < actualIndex) continue;
            if (toReturn != null && i > actualIndex && i > toReturnIndex && toReturnIndex > actualIndex) continue;
            toReturn = colorMatches[i];
            toReturnIndex = i;
        }
        return toReturn;
    }
    public ColorMatch GetPreviousFreeColor(ColorMatch actual)
    {
        ColorMatch toReturn = null;
        int toReturnIndex = 100;
        for (int i = colorMatches.Count-1, actualIndex = (actual == null) ? 100 : colorMatches.IndexOf(actual); i >= 0; i--)
        {
            if (colorMatches[i].pointed || i == actualIndex) continue;
            if (toReturn != null && i > actualIndex) continue;
            if (toReturn != null && i < actualIndex && i < toReturnIndex && toReturnIndex < actualIndex) continue;
            toReturn = colorMatches[i];
            toReturnIndex = i;
        }
        return toReturn;
    }

    public PlayerDisplayerManager GetNextFreeDisplayer()
    {
        List<GameObject> actives = playerDisplayers.FindAll(go => !go.GetComponent<PlayerDisplayerManager>().IsActive());
        if (actives.IsEmpty()) return null;
        actives = actives.OrderBy(go => go.name).ToList();
        return actives[0].GetComponent<PlayerDisplayerManager>();
    }

    private bool HasEveryoneChosen = false;
    void Update()
    {
        List<IGamepad> pressingA = GamepadManager.Instance.GetGamepadsByPressingButton(IGamepad.Key.ActionButtonDown, IGamepad.PressureType.Single);
        if (!pressingA.IsEmpty())
        {
            foreach (IGamepad gamepad in pressingA)
            {
                GameObject usedDisplayer = playerDisplayers.Find(d => d.GetComponent<PlayerDisplayerManager>().GetGamepadId() == gamepad.Id);
                if (usedDisplayer != null)
                {
                    // Gamepad already paired
                    Debug.Log("Already paired case");
                    PlayerDisplayerManager displayer = usedDisplayer.GetComponent<PlayerDisplayerManager>();
                    if (!displayer.IsConfirmed())
                        displayer.SetConfirm(true);
                }
                else
                {
                    Debug.Log("New pairing case");
                    // Gamepad to pair
                    PlayerDisplayerManager freeDisplayer = GetNextFreeDisplayer();
                    freeDisplayer.SetActive(true, gamepad);
                }
            }
        }
        if (GamepadManager.Instance.IsButtonPressedFromAnyGamepad(IGamepad.Key.Start, IGamepad.PressureType.Single) && !HasEveryoneChosen)
        {
            int countReady = playerDisplayers.FindAll(d => d.GetComponent<PlayerDisplayerManager>().IsConfirmed()).Count;
            if (countReady < 2) return;
            else
            {
                HasEveryoneChosen = true;
                StartCoroutine(StartGameChoiseRoom());
            }
        }
    }

    private IEnumerator StartGameChoiseRoom()
    {
        Log.Logger.Write("Player choise confirmed");
        GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomEnds();
        CreatePlayerDtos();
        yield return new WaitForSeconds(2);
        ConfirmAndGoToGameChoise();
    }

    private void CreatePlayerDtos()
    {
        List<GameObject> dispObjs = playerDisplayers.FindAll(d => d.GetComponent<PlayerDisplayerManager>().IsConfirmed());
        Log.Logger.Write($"Creating {dispObjs.Count} player data");
        int playerCounter = 1;
        foreach (GameObject dobj in dispObjs)
        {
            PlayerDisplayerManager pdm = dobj.GetComponent<PlayerDisplayerManager>();
            PlayerSelectorDto pDto = new PlayerSelectorDto()
            {
                ControllerId = pdm.GetGamepadId() ?? throw new System.ArgumentNullException("Cannot assign an empty controller id while generating player profiles."),
                PlayerNumber = playerCounter,
                ColorIndex = colorMatches.IndexOf(colorMatches.Find(cm => pdm.GetActualColorMatch().colorId == cm.colorId)),
                Color = (colorMatches.Find(cm => pdm.GetActualColorMatch().birdColor == cm.birdColor)).birdColor
            };
            playerDtos.Add(pDto);
            Log.Logger.Write($"Created player data for player {playerCounter}");
            playerCounter++;
        }
    }

    public void ConfirmAndGoToGameChoise()
    {
        AppSettings.Save("N_PLAYERS", playerDtos.Count);
        if (playerDtos.Count <= 4) Constants.VICTORY_POINTS = 5;
        else Constants.VICTORY_POINTS = 10;
        Log.Logger.Write($"Set the victories limit at {Constants.VICTORY_POINTS}");
        List<RankingDto.Rank> emptyRanks = new List<RankingDto.Rank>();
        for (int i = 0; i < playerDtos.Count; i++)
        {
            PlayerSelectorDto playerSelectorDto = playerDtos[i];
            AppSettings.Save("GAMEPAD_PLAYER" + playerSelectorDto.PlayerNumber, playerSelectorDto.ControllerId);
            Log.Logger.Write($"Saved GAMEPAD_PLAYER{playerSelectorDto.PlayerNumber}: {AppSettings.Get("GAMEPAD_PLAYER" + playerSelectorDto.PlayerNumber)}");
            AppSettings.Save("COLOR_PLAYER" + playerSelectorDto.PlayerNumber, colorMatches[playerSelectorDto.ColorIndex].prefab);
            Log.Logger.Write($"Saved COLOR_PLAYER{playerSelectorDto.PlayerNumber}: {AppSettings.Get("COLOR_PLAYER" + playerSelectorDto.PlayerNumber)}");
            AppSettings.Save("COLOR_RAW_PLAYER" + playerSelectorDto.PlayerNumber, Constants.BirdColorToString(playerSelectorDto.Color));
            Log.Logger.Write($"Saved COLOR_RAW_PLAYER{playerSelectorDto.PlayerNumber}: {Constants.BirdColorToString(playerSelectorDto.Color)}");
            emptyRanks.Add(new(playerSelectorDto.PlayerNumber, colorMatches[playerSelectorDto.ColorIndex].prefab.GetComponent<SpriteRenderer>().sprite));
        }
        AppSettings.Save(Constants.APPSETTINGS_RANKING_LABEL, RankingDto.Generate(emptyRanks));
        AppSettings.Save(Constants.APPSETTINGS_RANKING_PREVIOUS_LABEL, RankingDto.Generate(emptyRanks));
        Log.Logger.Write($"Generated ranks");
        SceneManager.LoadScene("GameLoader", LoadSceneMode.Single);
    }
}
