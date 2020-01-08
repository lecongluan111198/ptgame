using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectSerializedJson
{
    public int id { get; set; }
    public int level { get; set; }
    public int type { get; set; }
    public int state { get; set; }
    public int finishBuildingTime { get; set; }
    public float[] place { get; set; }
    public int size { get; set; }
    public MapObjectSerializedJson() { }

    public static string _MapObjectToSerializedJson(MapObject mapObject)
    {
        string result = JsonUtility.ToJson(mapObject);
        Debug.Log(result);
        return result;
    }

    public static MapObject _SerializedToMapObject(MapObjectSerialized mapObjectSerialized)
    {
        return new MapObject()
        {
            id = mapObjectSerialized.id,
            level = mapObjectSerialized.level,
            type = mapObjectSerialized.type,
            state = mapObjectSerialized.state,
            finishBuildingTime = mapObjectSerialized.finishBuildingTime,
            size = mapObjectSerialized.size,
            place = new Vector2(mapObjectSerialized.place[0], mapObjectSerialized.place[1])
        };
    }
}
