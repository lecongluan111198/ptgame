public class String
{
    // Paths
    public static string MAP_PATH = "sprites/maps/";
    public static string BUILDING_PATH = "sprites/buildings/";
    public static string BUILDING_CANON_PATH = "sprites/buildings/Canon/";
    public static string TROOP_PATH = "sprites/troops/";
    public static string CELL3X3_PATH = "sprites/maps/3x3/";

    // Files image
    public static string BASE_NORMAL = "base";
    public static string BARRACK = "barrack1";
    public static string CELL3X3_GROUND = "GROUND_3";
    public static string CELL3X3_SHADOW = "GRASS_3_Shadow";
    public static string CELL3X3_GREEN_GRID = "GREEN_3";
    public static string CELL3X3_RED_GRID = "RED_3";
    public static string CELL3X3_FENCE = "fence";
    public static string CELL3X3_ARROW_MOVE = "arrowmove";
    public static string TROOP_SHADOW = "shadow";
    public static string ARROW = "arrow";
    public static string ROCK = "rock";

    // Files data
    public static string MAP_DATA_FILE = "_MapData.DAT";
    public static string USER_INFO_FILE = "_UserInfo.DAT";

    // Name
    public static string TROOP_PREFIX = "ARM_";
    public static string MAP_OBJECT_PREFIX = "GO_";
    public static string MAP_OBJECT_GROUND = "GROUND_";
    public static string MAP_OBJECT_GREEN = "GREEN_";
    public static string MAP_OBJECT_RED = "RED_";
    public static string MAP_OBJECT_SHADOW = "SHADOW_";
    public static string MAP_OBJECT_FENCE = "FENCE_";
    public static string MAP_OBJECT_AROUND_MOVE = "AROUND_MOVE_";
    public static string USERNAME_DEFAULT = "Default Player";

    //host local
    //public static string SERVER_HOST = "http://localhost:9088/api";
    //host server
    public static string SERVER_HOST = "http://qui.tesosoft.com/api";

    //api 
    public static string API_LOGIN = SERVER_HOST + "/user/login";
    public static string API_LOAD_MAP = SERVER_HOST + "/map/{0}";
    public static string API_SAVE_MAP = SERVER_HOST + "/map/savefile-{0}";

    //scece
    public static string SCENE_LOADING = "LoadingMap";
    public static string SCENE_MAIN = "MainScene";

}
