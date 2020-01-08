using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrice : MonoBehaviour
{
    public static int getPrice(BuildingFactory.BuildingType type){
        switch (type) {
            case BuildingFactory.BuildingType.HOUSE:
                return 500;
            case BuildingFactory.BuildingType.STORAGE:
                return 100;
            case BuildingFactory.BuildingType.MINE:
                return 200;
            case BuildingFactory.BuildingType.BARRACK:
                return 300;
            case BuildingFactory.BuildingType.CAMP:
                return 400;
            case BuildingFactory.BuildingType.TOWNHALL:
                return 400;
            default:
                return 500;
        }
    }
}
