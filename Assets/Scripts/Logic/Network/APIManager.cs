using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Logic.Network
{
    class APIManager:MonoBehaviour
    {
        private static APIManager _instance;
        private static GameObject go;
        public static APIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    go = new GameObject();
                    _instance = go.AddComponent<APIManager>();
                }
                return _instance;
            }
        }

        public IEnumerator _Register(string userName, string passWord, Action<bool> callBack)
        {
            JSONObject account = new JSONObject();
            account.Add("userName", userName);
            account.Add("urlMap", "string");
            account.Add("passWord", passWord);

            _instance.StartCoroutine(APIRequest.Instance.doPost(API.REGISTER, account.ToString(), (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        callBack(true);
                    }
                    else
                    {
                        callBack(false);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _Login(string userName, string passWord,Action<bool> callBack)
        {
            JSONObject account = new JSONObject();
            account.Add("username", userName);
            account.Add("password", passWord);

            _instance.StartCoroutine(APIRequest.Instance.doPost(API.LOGIN, account.ToString(), (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        int level = rp.Response["level"].AsInt;

                        UserInfo.Instance.id = rp.Response["id"].AsInt;
                        UserInfo.Instance.userName = rp.Response["username"].Value;
                        UserInfo.Instance.Coin = rp.Response["coin"].AsInt;
                        UserInfo.Instance.Level = level;
                        UserInfo.Instance.Exp = rp.Response["exp"].AsDouble;
                        UserInfo.Instance.MaxExp = rp.Response["maxExp"].AsDouble;
                        UserInfo.Instance.Building = rp.Response["building"].AsInt;
                        UserInfo.Instance.Trooper = rp.Response["trooper"].AsInt;
                        UserInfo.Instance.Dynamon = rp.Response["dynamon"].AsInt;

                        UserInfo.Instance.MaxCoin = level*UserInfo._amountCoin;
                        UserInfo.Instance.MaxTrooper = level * UserInfo._amountTrooper;
                        UserInfo.Instance.MaxBuilding = level * UserInfo._amountBuilding;

                        callBack(true);
                    }
                    else
                    {
                        callBack(false);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _GetUserInfo(int userID, Action<bool> callBack)
        {
            string url = string.Format(API.LOAD_USER_INFO, userID);

            _instance.StartCoroutine(APIRequest.Instance.doGet(url, (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        int level = rp.Response["level"].AsInt;

                        Debug.Log(rp.Response["id"].AsInt);
                        Debug.Log(rp.Response["coin"].AsInt);
                        Debug.Log(rp.Response["exp"].AsDouble);
                        Debug.Log(rp.Response["building"].AsInt);
                        Debug.Log(rp.Response["trooper"].AsInt);
                        Debug.Log(rp.Response["username"].Value);
                        Debug.Log(rp.Response["id"].AsInt);

                        

                        callBack(true);
                    }
                    else
                    {
                        callBack(false);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _GetRandomUser(int userID, Action<string, int> callBack)
        {
            JSONObject currentUser = new JSONObject();
            currentUser.Add("userID", userID);
            string url = string.Format(API.LOAD_RANDOM_USER);

            _instance.StartCoroutine(APIRequest.Instance.doPost(url, currentUser.ToString(),(rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        int level = rp.Response["level"].AsInt;
                        // Debug.Log(rp.Response["id"].AsInt);
                        // Debug.Log(rp.Response["coin"].AsInt);
                        // Debug.Log(rp.Response["exp"].AsDouble);
                        // Debug.Log(rp.Response["building"].AsInt);
                        // Debug.Log(rp.Response["trooper"].AsInt);
                        // Debug.Log(rp.Response["username"].Value);
                        // Debug.Log(rp.Response["id"].AsInt);

                        callBack(rp.Response["username"].Value, rp.Response["id"].AsInt);
                    }
                    else
                    {
                        callBack(string.Empty, -1);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _UploadMap(int userID, string path)
        {
            string url = string.Format(API.SAVE_USER_MAP, userID);
            yield return null;
            Debug.Log(url);
            _instance.StartCoroutine(APIRequest.Instance.uploadFile(url, path, (rp) =>
            {
                if (rp)
                {
                    Debug.Log(API.SUCCES_CODE);
                }
                else
                {
                    Debug.Log(API.ERROR_CONNECT);
                }
            }));
            yield return null;
        }

        public IEnumerator _DowloadMap(int userID,string path,Action<bool> callBack)
        {
            string url = string.Format(API.LOAD_USER_MAP, userID);

            yield return null;
            Debug.Log(url);
            _instance.StartCoroutine(APIRequest.Instance.downloadFile(url, path, (rp) =>
            {
                if (rp)
                {
                    Debug.Log(API.SUCCES);
                    callBack(true);
                }
                else
                {
                    Debug.Log(API.ERROR_CONNECT);
                    callBack(false);
                }
            }));
            yield return null;
        }

        public IEnumerator _GetUserTrooper(int userID, Action<bool> callBack)
        {
            string url = string.Format(API.LOAD_USER_TROOPER, userID);
            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doGet(url, (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        for (int i = 0; i < rp.Response.AsArray.Count;i++)
                        {
                            int id = rp.Response.AsArray[i]["trooper"]["id"].AsInt;

                            if (id == 1) {
                                int num = rp.Response.AsArray[i]["count"].AsInt;
                                for (int j = 0; j < num; j++) {
                                    Troop troop = new ARM_1(i * 1000 + j, false);
                                    TroopManager.instance.listTroop.Add(troop);
                                }
                            }
                            else {
                                int num = rp.Response.AsArray[i]["count"].AsInt;
                                for (int j = 0; j < num; j++) {
                                    Troop troop = new ARM_2(i * 1000 + j, false);
                                    TroopManager.instance.listTroop.Add(troop);
                                }
                            }

                            // //Lấy số lượng lính
                            // Debug.Log(rp.Response.AsArray[i]["count"].AsInt);

                            // //Lấy thông tin lính
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["id"].AsInt);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["name"].Value);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["dame"].AsDouble);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["speed"].AsDouble);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["price"].AsInt);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["spornTime"].AsDouble);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["type"].AsInt);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["rangeAttack"].AsDouble);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["speedAttack"].AsDouble);
                            // Debug.Log(rp.Response.AsArray[i]["trooper"]["hp"].AsInt);
                        }
                        callBack(true);
                    }
                    else {
                        callBack(false);
                    }
                }
            }));

            yield return null;
        }

        public IEnumerator _GetUserBuilding(int userID)
        {
            string url = string.Format(API.LOAD_USER_BUILDING, userID);
            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doGet(url, (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        for (int i = 0; i < rp.Response.AsArray.Count; i++)
                        {
                            //Lấy số lượng lính
                            Debug.Log(rp.Response.AsArray[i]["count"].AsInt);

                            //Lấy thông tin lính
                            Debug.Log(rp.Response.AsArray[i]["building"]["id"].AsInt);
                            Debug.Log(rp.Response.AsArray[i]["building"]["name"].Value);
                            Debug.Log(rp.Response.AsArray[i]["building"]["dame"].AsDouble);
                            Debug.Log(rp.Response.AsArray[i]["building"]["speed"].AsDouble);
                            Debug.Log(rp.Response.AsArray[i]["building"]["price"].AsInt);
                            Debug.Log(rp.Response.AsArray[i]["building"]["buildTime"].AsDouble);
                            Debug.Log(rp.Response.AsArray[i]["building"]["type"].AsInt);
                            Debug.Log(rp.Response.AsArray[i]["building"]["rangeAttack"].AsDouble);
                            Debug.Log(rp.Response.AsArray[i]["building"]["capacity"].AsInt);

                        }
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _IncreaseUserCoin(int userID,int coin)
        {
            string url = string.Format(API.INCREASE_USER_COIN);

            JSONObject account = new JSONObject();
            account.Add("coin", coin);
            account.Add("userID", userID);

            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doPost(url, account.ToString(),(rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        Debug.Log(rp.Response["coin"].AsInt);
                        Debug.Log(rp.Response["level"].AsInt);
                        Debug.Log(rp.Response["exp"].AsDouble);
                        Debug.Log(rp.Response["id"].AsInt);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _DecreaseUserCoin(int userID, int coin)
        {
            string url = string.Format(API.DECREASE_USER_COIN);

            JSONObject account = new JSONObject();
            account.Add("coin", coin);
            account.Add("userID", userID);

            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doPost(url, account.ToString(), (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        Debug.Log(rp.Response["coin"].AsInt);
                        Debug.Log(rp.Response["level"].AsInt);
                        Debug.Log(rp.Response["exp"].AsDouble);
                        Debug.Log(rp.Response["id"].AsInt);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _IncreaseUserEXP(int userID, int exp)
        {
            string url = string.Format(API.INCREASE_USER_EXP);

            JSONObject account = new JSONObject();
            account.Add("exp", exp);
            account.Add("userID", userID);

            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doPost(url, account.ToString(), (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        Debug.Log(rp.Response["coin"].AsInt);
                        Debug.Log(rp.Response["level"].AsInt);
                        Debug.Log(rp.Response["exp"].AsDouble);
                        Debug.Log(rp.Response["id"].AsInt);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _DecreaseUserTrooper(int userID, Dictionary<string,int> trooperList)
        {
            //example value
            Dictionary<string, int> example = new Dictionary<string, int>();
            example.Add("1", 1);
            example.Add("2", 1);
            example.Add("3", 1);

            JSONObject objectTrooper = new JSONObject();
            foreach (KeyValuePair<string, int> item in example)
            {
                objectTrooper.Add(item.Key, item.Value);
            }

            string url = string.Format(API.DECREASE_USER_TROOPER,userID);

            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doPost(url, objectTrooper.ToString(), (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        Debug.Log(rp.Response.Value);
                    }
                }
            }));
            yield return null;
        }

        public IEnumerator _IncreaseUserTrooper(int userID, Dictionary<string, int> trooperList)
        {
            //example value
            Dictionary<string, int> example = new Dictionary<string, int>();
            example.Add("1", 1);
            example.Add("2", 1);
            example.Add("3", 1);

            JSONObject objectTrooper = new JSONObject();
            foreach (KeyValuePair<string, int> item in example)
            {
                objectTrooper.Add(item.Key, item.Value);
            }

            string url = string.Format(API.INCREASE_USER_TROOPER, userID);

            yield return null;
            _instance.StartCoroutine(APIRequest.Instance.doPost(url, objectTrooper.ToString(), (rp) =>
            {
                if (rp != null)
                {
                    if (rp.StatusCode == API.SUCCES_CODE)
                    {
                        Debug.Log(rp.Response.Value);
                    }
                }
            }));
            yield return null;
        }


        #region one way function

        public void _oneWayDecreaseMoney(int coin)
        {
            StartCoroutine(_DecreaseUserCoin(UserInfo.Instance.id, coin));
        }

        public void _oneWayIncreaseMoney(int coin)
        {
            StartCoroutine(_IncreaseUserCoin(UserInfo.Instance.id, coin));
        }

        #endregion
    }
}
