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
        public bool pointed = false;
    }

    [System.Serializable]
    public class PlayerSelectorDto
    {
        public int PlayerNumber;
        public int ControllerId;
        public int ColorIndex;
    }

    public List<PlayerSelectorDto> playerDtos;
    public List<ColorMatch> colorMatches;
    public List<GameObject> playerDisplayers;

    private void Awake()
    {
        _instance = this;
    }

    public ColorMatch GetNextFreeColor()
    {
        List<ColorMatch> actives = colorMatches.FindAll(c => c.pointed == false);
        if (actives.IsEmpty()) return null;
        actives = actives.OrderBy(c => c.colorId).ToList();
        return actives[0];
    }

    public PlayerDisplayerManager GetNextFreeDisplayer()
    {
        List<GameObject> actives = playerDisplayers.FindAll(go => !go.GetComponent<PlayerDisplayerManager>().IsActive());
        if (actives.IsEmpty()) return null;
        actives = actives.OrderBy(go => go.name).ToList();
        return actives[0].GetComponent<PlayerDisplayerManager>();
    }

    void Start()
    {
        
    }


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
    }

    public void ConfirmAndGoToGameChoise()
    {
        AppSettings.Save("N_PLAYERS", playerDtos.Count);
        for (int i = 0; i < playerDtos.Count; i++)
        {
            PlayerSelectorDto playerSelectorDto = playerDtos[i];
            AppSettings.Save("GAMEPAD_PLAYER" + playerSelectorDto.PlayerNumber, playerSelectorDto.ControllerId);
            AppSettings.Save("COLOR_PLAYER" + playerSelectorDto.PlayerNumber, colorMatches[playerSelectorDto.ColorIndex].prefab);
        }
        SceneManager.LoadScene("GameLoader", LoadSceneMode.Single);
    }
}
