public class Number {
    public static int MAP_SIZE = 40;

    // Object type
    public static int MAP_OBJECT_TYPE_HOUSE = 0;
    public static int MAP_OBJECT_TYPE_STORAGE = 1;
    public static int MAP_OBJECT_TYPE_MINE = 2;
    public static int MAP_OBJECT_TYPE_BARRACK = 3;
    public static int MAP_OBJECT_TYPE_CAMP = 4;
    public static int MAP_OBJECT_TYPE_TOWNHALL = 5;

    // Object size corresponding with type (see above)
    public static int[] MAP_OBJECT_SIZE = new int[] { 4, 3, 3, 3, 5, 4, 3 };

    // Object scale corresponding with size (see above)
    public static float[] MAP_OBJECT_SCALE_SIZE = new float[] { 0, 0.045F, 0.095F, 0.145F, 0.195F, 0.245F, 0,  0 };
    public static float TROOP_SCALE_SIZE = 0.35F;

    //Object's layers of sprite component.
    public static int MAP_OBJECT_LAYER_ORDER = 1;
    public static int MAP_OBJECT_LAYER_GROUND = 1;
    public static int MAP_OBJECT_LAYER_SHADOW = 2;
    public static int MAP_OBJECT_LAYER_GREEN_GRID = 3;
    public static int MAP_OBJECT_LAYER_RED_GRID = 4;
    public static int MAP_OBJECT_LAYER_FENCE = 5;
    public static int MAP_OBJECT_LAYER_SPRITE = 6;

    public static float MAP_OBJECT_PIVOT_X = 0.5F;
    public static float MAP_OBJECT_PIVOT_Y = 0.0F;

    // Troop config
    public static int ARM_IDLE_STATE = 0;
    public static int ARM_RUN_STATE = 1;
    public static int ARM_ATTACK_STATE = 2;
    public static int[] ARM_1_ANIM_FRAME = new int[] { 6, 14, 13 };
    public static int[] ARM_2_ANIM_FRAME = new int[] { 6, 16, 13 };

    public static int DIRECTION_E = 0;
    public static int DIRECTION_NE = 1;
    public static int DIRECTION_N = 2;
    public static int DIRECTION_NW = 3;
    public static int DIRECTION_W = 4;
    public static int DIRECTION_SW = 5;
    public static int DIRECTION_S = 6;
    public static int DIRECTION_SE = 7;

    public static int ARM_1_INDEX = 0;
    public static int ARM_2_INDEX = 1;
    public static int ARM_3_INDEX = 2;
    public static int ARM_4_INDEX = 3;

    // Object state
    public static int MAP_NORMAL_STATE = 1;
    public static int MAP_DEAD_STATE = 2;
    public static int MAP_ACTTACK_STATE = 3;
}