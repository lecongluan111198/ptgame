using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollItem : MonoBehaviour {
    [SerializeField]
    private Text TitleText;
    public ItemController.ItemType type;
    private List<GameObject> listItem;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start () {
        listItem = ItemController.instance.GetItems (type);
    }

    public void _ShowAll () {
        //forward to PopulateGrid
        PopulateGrid.instance._Populate (listItem);
    }
    public void _SetTitle (string title) {
        TitleText.text = title;
    }
    public List<GameObject> _GetItems () {
        return listItem;
    }
}