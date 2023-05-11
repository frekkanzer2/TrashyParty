public static class Constants {
    public static int PLAYER_MOVEMENT_SPEED = 15;
    public static int PLAYER_JUMPING_POWER = 38;
    public static int LAYER_DEADZONE = 29;
    public static int LAYER_GROUND = 6;
    public static int LAYER_PLAYERHEAD = 31;
    public static int VICTORY_POINTS = 5;
    public static string LEVEL_SPECIFIC_SOUND_TAG = "LevelSound";
    public static string ANIMATION_PRESENTATION_STATE = "Presentation";
    public static string APPSETTINGS_PLAYABLEGAMES_LABEL = "PLAYABLE_GAMES";
    public static string APPSETTINGS_RANKING_LABEL = "RANKING";
    public static string APPSETTINGS_RANKING_PREVIOUS_LABEL = "RANKING_PREV";
    public enum BirdColor
    {
        Red,
        Blue,
        Green,
        Grey,
        Orange,
        Pink,
        Sky,
        Yellow
    }
    public static string BirdColorToString(BirdColor color) => System.Enum.GetName(color.GetType(), color).ToUpper();

    public enum GameName
    {
        BeachVolley,
        CloudyBoxes,
        LavaDodge,
        EnergyRelease,
        BirdsFootsteps,
        Electroshock,
        HeadSmash,
        DeadlyDrop,
        TappyBird,
        UfoInvasion,
        ImTheKing,
        BirdSoccer,
        CatDance,
        Basketegg,
        LaboratoryLights,
        DetonationBird,
        FizzleFloor,
        UnderTheRain,
        CatchUp,
        BombTag,
        SuperHot
    }
}
