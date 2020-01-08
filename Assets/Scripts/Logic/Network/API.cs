using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API
{

    public static string ERROR_CONNECT = "connection error";
    public static string SUCCES = "success";

    public static int SUCCES_CODE = 200;

    //host local
    //public static string SERVER_HOST = "http://localhost:9088/api";
    //host server
    public static string SERVER_HOST = "http://qui.tesosoft.com/api";

    public static string LOGIN = SERVER_HOST + "/user/login";
    public static string REGISTER = SERVER_HOST + "/user/register";
    public static string LOAD_USER_MAP = SERVER_HOST + "/map/{0}";
    public static string SAVE_USER_MAP = SERVER_HOST + "/map/savefile-{0}";
    public static string LOAD_USER_INFO = SERVER_HOST + "/user/{0}/info";
    public static string LOAD_RANDOM_USER = SERVER_HOST + "/user/random";


    public static string LOAD_ALL_TROOPER = SERVER_HOST + "/trooper/get-all";
    public static string DECREASE_USER_TROOPER = SERVER_HOST + "/trooper/user-{0}/decrease-trooper";
    public static string LOAD_USER_TROOPER = SERVER_HOST + "/trooper/user-{0}/get-all";
    public static string INCREASE_USER_TROOPER = SERVER_HOST + "/trooper/user-{0}/increase-trooper";

    public static string LOAD_ALL_BUILDING = SERVER_HOST + "/building/get-all";
    public static string LOAD_USER_BUILDING = SERVER_HOST + "/building/user-{0}/get-all";
    public static string INCREASE_USER_BUILDING = SERVER_HOST + "/building/user-{0}/increase-building";
    public static string DECREASE_USER_BUILDING = SERVER_HOST + "/building/user-{0}/decrease-building";

    public static string DECREASE_USER_COIN = SERVER_HOST + "/user/decrease-coin";
    public static string INCREASE_USER_COIN = SERVER_HOST + "/user/add-coin";
    public static string INCREASE_USER_EXP = SERVER_HOST + "/user/experience";
}
