using UnityEngine;
using UnityEngine.SceneManagement;

public class MockGameLoader : MonoBehaviour, IGameLoader
{
    public void LoadGame()
    {
        Constants.GameName game = (Constants.GameName)AppSettings.Get("CHOSEN_GAME");
        int players = (int)AppSettings.Get("N_PLAYERS");
        string sceneName = "game_";
        switch(game)
        {
            case Constants.GameName.BeachVolley:
                sceneName += "beachvolley";
                break;
            case Constants.GameName.RocketBirdLeague:
                sceneName += "rocketbirdleague";
                break;
            case Constants.GameName.CloudyBoxes:
                sceneName += "cloudyboxes";
                break;
            case Constants.GameName.LavaDodge:
                sceneName += "lavadodge";
                break;
            case Constants.GameName.EnergyRelease:
                sceneName += "energyrelease";
                break;
            case Constants.GameName.BirdsFootsteps:
                sceneName += "birdsfootsteps";
                break;
            case Constants.GameName.Electroshock:
                sceneName += "electroshock";
                break;
            case Constants.GameName.HeadSmash:
                sceneName += "headsmash";
                break;
            case Constants.GameName.DeadlyDrop:
                sceneName += "deadlydrop";
                break;
            case Constants.GameName.TappyBird:
                sceneName += "tappybird";
                break;
            default:
                throw new System.NullReferenceException($"No game named {game} is registered inside the loading component");
        }
        Debug.Log($"Loading game {game} on scene {sceneName} with {players} players");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
