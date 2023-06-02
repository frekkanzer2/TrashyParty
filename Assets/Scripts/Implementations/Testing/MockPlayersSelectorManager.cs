using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MockPlayersSelectorManager : MonoBehaviour, IPlayersSelectorManager
{
    public void ConfirmAndGoToGameChoise()
    {
        AppSettings.Save("N_PLAYERS", playerDtos.Count);
        for(int i = 0; i < playerDtos.Count; i++)
        {
            MockPlayerSelectorDto mockDto = playerDtos[i];
            AppSettings.Save("GAMEPAD_PLAYER" + mockDto.PlayerNumber, mockDto.ControllerId);
            AppSettings.Save("COLOR_PLAYER" + mockDto.PlayerNumber, colorPrefabs[mockDto.ColorIndex]);
        }
        SceneManager.LoadScene("GameLoader", LoadSceneMode.Single);
    }

    [System.Serializable]
    public class MockPlayerSelectorDto
    {
        public int PlayerNumber;
        public int ControllerId;
        public int ColorIndex;
    }

    public List<MockPlayerSelectorDto> playerDtos;
    public List<GameObject> colorPrefabs;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Singleton<ILogManager>.Instance.Write("MockPlayersSelectorManager > Press 'S' to load player testing data and to start the game");
#endif
    }

    private bool confirmed = false;

    // Update is called once per frame
    void Update()
    {
        if (!confirmed)
            if (Input.GetKeyDown(KeyCode.S))
            {
                confirmed = true;
                ConfirmAndGoToGameChoise();
            }
    }
}
