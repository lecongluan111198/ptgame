using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Logic.Network;

public class UserInfo
{

    #region local data

    public static int _amountTrooper = 50;
    public static int _amountBuilding = 50;
    public static int _amountCoin = 150000;
   
    #endregion

    public string userName { get; set; }
    public int id { get; set; }
    private static UserInfo _instance;
    public string path = Application.persistentDataPath + String.USER_INFO_FILE;

    public int Level { get; set; }
    public double Exp { get; set; }
    public double MaxExp { get; set; }

    public int Coin { get; set; }
    public int Building { get; set; }
    public int Trooper { get; set; }
    public int Dynamon { get; set; }

    public int MaxCoin { get; set; }
    public int MaxBuilding { get; set; }
    public int MaxTrooper { get; set; }

    public static UserInfo Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UserInfo();
            }
            return _instance;
        }
    }

    public void decreaseMoney(int coin)
    {
        this.Coin -= coin;
        APIManager.Instance._oneWayDecreaseMoney(coin);
    }

    public void increaseMoney(int coin)
    {
        if (this.Coin + coin >= _amountCoin) {
            coin = _amountCoin - this.Coin;
        }
        
        this.Coin += coin;
        APIManager.Instance._oneWayIncreaseMoney(coin);
    }
}
