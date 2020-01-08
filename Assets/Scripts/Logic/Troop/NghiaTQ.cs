using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NghiaTQ : MonoBehaviour
{
    private float intervalRefreshTroop = 0.075F;
    private float timeToRefreshTroop = 0;

    void Start()
    {
        if (TroopManager.instance == null) TroopManager.instance = new TroopManager();
    }

    void Update()
    {
        if (Time.time > timeToRefreshTroop)
        {
            timeToRefreshTroop += intervalRefreshTroop;
            TroopManager.instance._Update();
        }
        if (Input.GetMouseButtonDown(0))
        {
           
        }
    }
}
