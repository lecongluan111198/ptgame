using Assets.Scripts.Logic.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public Text userLevelTxt, userLevelEXPTxt, userCoinTxt, userTrooperTxt,
        userMaxCoinTxt, userMaxTrooperTxt, userMaxBuildingTxt, userBuildingTxt, userDynamonTxt;

    public Image coinBar, trooperBar, buildingBar, dynamonBar, expBar;

    public GameObject InfoPanel;

    public GameObject BuildingAction;

    // Start is called before the first frame update
    void Start()
    {
        _loadAllUserInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void _showBuildingInfo()
    {
        InfoPanel.SetActive(true);
    }

    public void _hideBuildingInfo()
    {
        InfoPanel.SetActive(false);
    }

    public void _showBuildingAction()
    {
        BuildingAction.SetActive(true);
        //StartCoroutine(buildingMoveTop());
    }

    IEnumerator buildingMoveTop()
    {
        Vector2 current = BuildingAction.transform.position;
        current.y += 50f;
        yield return new WaitForSeconds(1f);
        BuildingAction.transform.position = current;
        yield return null;
    }

    public void _hideBuidlingAciont()
    {
        BuildingAction.SetActive(false);
    }

    public void _loadAllUserInfo()
    {
        userLevelTxt.text = UserInfo.Instance.Level.ToString();
        userLevelEXPTxt.text = UserInfo.Instance.Exp.ToString();

        userCoinTxt.text = UserInfo.Instance.Coin.ToString();
        userTrooperTxt.text = UserInfo.Instance.Trooper.ToString();
        userBuildingTxt.text = UserInfo.Instance.Building.ToString();
        userDynamonTxt.text = UserInfo.Instance.Dynamon.ToString();

        userMaxCoinTxt.text = UserInfo.Instance.MaxCoin.ToString();
        userMaxTrooperTxt.text = UserInfo.Instance.MaxTrooper.ToString();
        userMaxBuildingTxt.text = UserInfo.Instance.MaxBuilding.ToString();


        coinBar.fillAmount = (float)UserInfo.Instance.Coin / UserInfo.Instance.MaxCoin;
        trooperBar.fillAmount = (float)UserInfo.Instance.Trooper / UserInfo.Instance.MaxTrooper;
        buildingBar.fillAmount = (float)UserInfo.Instance.Building / UserInfo.Instance.MaxBuilding;
        Debug.Log("================================="+ buildingBar.fillAmount);
        dynamonBar.fillAmount = 1f;
        expBar.fillAmount = (float)(UserInfo.Instance.Exp / UserInfo.Instance.MaxExp);

    }
}
