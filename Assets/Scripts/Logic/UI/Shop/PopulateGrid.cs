using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateGrid : MonoBehaviour {
    public GameObject item;
    public static PopulateGrid instance = null;
    private List<GameObject> currentItems = new List<GameObject> ();

    private void Awake () {
        if (instance == null) {
            //DontDestroyOnLoad (gameObject);
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public void _Populate (List<GameObject> item) {
        _ClearContent ();
        for (int i = 0; i < item.Count; i++) {
            currentItems.Add (Instantiate (item[i], transform));
        }
    }

    public void _ClearContent () {
        for (int i = 0; i < currentItems.Count; i++) {
            Destroy (currentItems[i]);
        }
    }
}