using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MapDataSerialized
{
    public int[,] data;
    public MapObjectSerialized[] objects;

    public static MapDataSerialized _MapDataToSerialized(MapData mapData) {

        // Initialize
        MapDataSerialized mapDataSerialized = new MapDataSerialized();
        mapDataSerialized.data = new int [Number.MAP_SIZE, Number.MAP_SIZE];
        mapDataSerialized.objects = new MapObjectSerialized[mapData.objects.Count];

        // Deep copy value
        for (int i = 0; i < Number.MAP_SIZE; i++) {
            for (int j = 0; j < Number.MAP_SIZE; j++) {
                mapDataSerialized.data[i,j] = mapData.data[i,j];
            }
        }
        for (int k = 0; k < mapData.objects.Count; k++) {
            mapDataSerialized.objects[k]= MapObjectSerialized._MapObjectToSerialized(mapData.objects[k]);
        }

        return mapDataSerialized;
    }

    public static MapData _SerializedToMapData(MapDataSerialized mapDataSerialized) {

        // Use the singleton
        for (int i = 0; i < Number.MAP_SIZE; i++) {
            for (int j = 0; j < Number.MAP_SIZE; j++) {
                MapData.instance.data[i,j] = mapDataSerialized.data[i,j];
            }
        }
        for (int k = 0; k < mapDataSerialized.objects.Length; k++) {
            MapData.instance.objects.Add(MapObjectSerialized._SerializedToMapObject(mapDataSerialized.objects[k]));
        }

        return MapData.instance;
    }
}
