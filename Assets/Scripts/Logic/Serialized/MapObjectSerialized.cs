using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MapObjectSerialized {
    public int id { get; set; }
    public int level { get; set; }
    public int type { get; set; }
    public int state { get; set; }
    public int finishBuildingTime { get; set; }
    public float[] place { get; set; }
    public int size { get; set; }
    public MapObjectSerialized() {}

    public static MapObjectSerialized _MapObjectToSerialized(MapObject mapObject) {

        return new MapObjectSerialized() {
            id = mapObject.id,
            level = mapObject.level,
            type = mapObject.type,
            state = mapObject.state,
            finishBuildingTime = mapObject.finishBuildingTime,
            size = mapObject.size,
            place = new float[2] { mapObject.place.x, mapObject.place.y }
        };
    }

    public static MapObject _SerializedToMapObject(MapObjectSerialized mapObjectSerialized) {
        return new MapObject() {
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