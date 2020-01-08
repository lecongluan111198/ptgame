using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject ShopUI;
    public GameObject SettingUI;
    public static UIManager instance = null;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake () {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad (gameObject);
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void _OpenSetting()
    {
        SettingUI.SetActive(true);
    }

    public void _CloseSetting()
    {
        SettingUI.SetActive(false);
    }

    public void _OpenShop () {
        ShopUI.SetActive (true);
    }
    public void _CloseShop () {
        ShopUI.SetActive (false);
    }
}