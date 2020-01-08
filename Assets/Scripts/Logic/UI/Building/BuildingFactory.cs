using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : MonoBehaviour {
    private static int count = 1;
    public static BuildingFactory instance = null;
    [SerializeField]
    private GameObject barrack, townhall, mine, storage, canon;
    public enum BuildingType {
        HOUSE = 0,
        STORAGE = 1,
        MINE = 2,
        BARRACK = 3,
        CAMP = 4,
        TOWNHALL = 5,

        CANON = 6,
    }
    void Awake () {
        if (instance == null) {
            //DontDestroyOnLoad (gameObject);
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public GameObject createBuilding (BuildingFactory.BuildingType type, Transform bTransform) {
        Vector3 position = bTransform.position;
        position.z = 0;
        switch (type) {
            case BuildingType.HOUSE:
            case BuildingType.STORAGE:
                return Instantiate (storage, position, bTransform.rotation);
            case BuildingType.MINE:
                return Instantiate (mine, position, bTransform.rotation);
            case BuildingType.BARRACK:
                return Instantiate (barrack, position, bTransform.rotation);
            case BuildingType.CAMP:
            case BuildingType.TOWNHALL:
                return Instantiate (townhall, position, bTransform.rotation);
            case BuildingType.CANON:
                return Instantiate (canon, position, bTransform.rotation);
            default:
                return Instantiate (townhall, position, bTransform.rotation);
        }
    }
}