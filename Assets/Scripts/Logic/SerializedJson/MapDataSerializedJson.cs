using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataSerializedJson
{
    public static string _MapDataToSerializedJson(MapData mapData)
    {
        string result = JsonUtility.ToJson(mapData);
        foreach(var a in mapData.objects)
        {
            MapObjectSerializedJson._MapObjectToSerializedJson(a);
        }
        return result;
    }

    public static MapData _JsonSerializedToMapData(String mapDataSerialized)
    {

        return MapData.instance;
    }
}
