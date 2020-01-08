using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeafItem : MonoBehaviour {
    [SerializeField]
    private Text PriceText, TitleText;
    [SerializeField]
    private Image image;
    [SerializeField]
    private BuildingFactory.BuildingType typeObject;

    private int currentPrice;

    private void Start()
    {
        currentPrice = BuildingPrice.getPrice(typeObject);
        _SetPrice(currentPrice);
    }

    public void _SetPrice (int price) {
        PriceText.text = price.ToString ();
    }

    public void _SetImage (Sprite sprite) {
        image.sprite = sprite;
    }

    public void _SetTitle (string title) {
        TitleText.text = title;
    }

    public void _AddToMapData () {
        if(UserInfo.Instance.Coin >= currentPrice)
        {
            //UPDATE UI
            UserInfo.Instance.decreaseMoney(currentPrice);

            //UPDATE
            GameObject.FindObjectOfType<MainUIController>()._loadAllUserInfo();

            int size = Number.MAP_OBJECT_SIZE[(int)typeObject];
            MapData.instance._AddMapObject(typeObject, 1, MapData.instance._FindEmptyPlace(size));
            UIManager.instance._CloseShop();

            //SAVE TO SERVER
            GamePlayController.instance.saveMap();
        }
        else
        {
            Debug.Log("not enough money");
        }
        
    }

    public bool isValid () {
        return true;
    }
}