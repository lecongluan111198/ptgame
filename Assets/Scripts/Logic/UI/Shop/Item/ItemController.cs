using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
    [SerializeField]
    private List<GameObject> Army;
    public static ItemController instance = null;

    public enum ItemType {
        TREASURY,
        RESOURCE,
        ARMY,
        DECORATOR,
        DEFENSE,
        PROTECTION,

    }

    private void Awake () {
        if (instance == null) {
            //DontDestroyOnLoad (gameObject);
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public List<GameObject> GetItems (ItemType type) {
        switch (type) {
            case ItemType.TREASURY:
            case ItemType.RESOURCE:
            case ItemType.ARMY:
                return Army;
            case ItemType.DECORATOR:
            case ItemType.DEFENSE:
            case ItemType.PROTECTION:
            default:
                return Army;
        }
    }
}