using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Info");
        GameObject UIM = GameObject.Find("MainLayoutController");
        UIM.GetComponent<MainUIController>()._showBuildingInfo();
    }
}
