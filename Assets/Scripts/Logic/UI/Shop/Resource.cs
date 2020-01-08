using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour {
    [SerializeField]
    private Text AmountText;

    [SerializeField]
    private Text HouseText;
    private int amount;
    // Start is called before the first frame update
    void Start () {
        setAmount(UserInfo.Instance.Coin);
        HouseText.text = UserInfo.Instance.Building + " / " + UserInfo.Instance.MaxBuilding;
    }

    // Update is called once per frame
    void Update () {

    }

    public void setAmount (int amount) {
        this.amount = amount;
        this.AmountText.text = this.amount.ToString ();
    }

    public void addAmount (int amount) {
        this.amount += amount;
        this.AmountText.text = this.amount.ToString ();
    }
}