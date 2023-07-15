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
            case Constants.GameName.UfoInvasion:
                sceneName += "ufoinvasion";
                break;
            case Constants.GameName.ShootyPerry:
                sceneName += "shootyperry";
                break;
            case Constants.GameName.ImTheKing:
                sceneName += "imtheking";
                break;
            case Constants.GameName.ColorfulNests:
                sceneName += "colorfulnests";
                break;
            case Constants.GameName.BirdSoccer:
                sceneName += "birdsoccer";
                break;
            case Constants.GameName.CatDance:
                sceneName += "catdance";
                break;
            case Constants.GameName.Basketegg:
                sceneName += "basketegg";
                break;
            case Constants.GameName.LaboratoryLights:
                sceneName += "laboratorylights";
                break;
            case Constants.GameName.DetonationBird:
                sceneName += "detonationbird";
                break;
            case Constants.GameName.FizzleFloor:
                sceneName += "fizzlefloor";
                break;
            case Constants.GameName.UnderTheRain:
                sceneName += "undertherain";
                break;
            case Constants.GameName.CatchUp:
                sceneName += "catchup";
                break;
            case Constants.GameName.BombTag:
                sceneName += "bombtag";
                break;
            case Constants.GameName.SuperHot:
                sceneName += "superhot";
                break;
            case Constants.GameName.TrapRun:
                sceneName += "traprun";
                break;
            case Constants.GameName.StaticStun:
                sceneName += "staticstun";
                break;
            case Constants.GameName.EggsRush:
                sceneName += "eggsrush";
                break;
            case Constants.GameName.GCEA:
                sceneName += "gottacatchemall";
                break;
            case Constants.GameName.HottieFloor:
                sceneName += "hottiefloor";
                break;
            case Constants.GameName.KagomeKagome:
                sceneName += "kagomekagome";
                break;
            case Constants.GameName.EggHatching:
                sceneName += "egghatching";
                break;
            default:
                throw new System.NullReferenceException($"No game named {game} is registered inside the loading component");
        }
        Log.Logger.Write($"Loading game {game} on scene {sceneName} with {players} players");
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
