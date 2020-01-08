using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class MapData {
    public int[, ] data;
    public List<MapObject> objects;
    public MapObject selectedObject;
    public static MapData instance;
    public static string path = Application.persistentDataPath + "_" + UserInfo.Instance.userName + String.MAP_DATA_FILE;
    public static string rivalPath = Application.persistentDataPath + "_Rival" + String.MAP_DATA_FILE;

    public MapData () {
        data = new int[Number.MAP_SIZE, Number.MAP_SIZE];
        objects = new List<MapObject> ();

        // Init values
        selectedObject = null;
        for (int i = 0; i < Number.MAP_SIZE; i++) {
            for (int j = 0; j < Number.MAP_SIZE; j++) {
                data[i, j] = -1;
            }
        }

        // Init node data
        PathFinding._InitNodeData ();
    }

    // Init map for new user
    private void _InitFirstTime () {
        // This is bad code
        //this._AddMapObject (BuildingFactory.BuildingType.CAMP, 1, new Vector2 (19, 9));
        this._AddMapObject (BuildingFactory.BuildingType.BARRACK, 1, new Vector2 (30, 25));
       // this._AddMapObject(BuildingFactory.BuildingType.STORAGE, 1, new Vector2(60, 25));
        this._AddMapObject(BuildingFactory.BuildingType.STORAGE, 1, new Vector2(20, 25));
      this._AddMapObject(BuildingFactory.BuildingType.MINE, 1, new Vector2(16, 25));
    }

    public void _AddMapObject (BuildingFactory.BuildingType _type, int _level, Vector2 _place) {

        // If place is invalid, inform user
        if (_place.x == -1 || _place.y == -1) {

            NotifyBoardUI.Instance._Active("Đã hết đất trống", "Vui lòng di chuyển các công trình hiện tại để tạo đất trống");

            return;
        }
        // Create an object with type and level
        MapObject mapObject = new MapObject() {
            type = (int) _type,
            id = objects.Count,
            size = Number.MAP_OBJECT_SIZE[(int) _type],
            level = _level,
            place = _place,
            spriteUI = BuildingFactory.instance.createBuilding (_type, CameraManager.instance.transform)
        };
        //choose right CellFactory of UI of MapObject
        // if (mapObject.size == 3)
        //     mapObject.spriteUI = new BuilderUI (new Cell3x3Factory (), _type, Number.MAP_OBJECT_SIZE[_type]);

        // If there is a slot for the object
        if (mapObject.place != null) {
            // Add it to list objects
            objects.Add (mapObject);

            // Check on data
            for (float i = mapObject.place.x; i > mapObject.place.x - mapObject.size; i--) {
                for (float j = mapObject.place.y; j > mapObject.place.y - mapObject.size; j--) {
                    data[(int) i, (int) j] = mapObject.id;
                }
            }
            mapObject._LoadGUI ();
        }

    }

    // TODO: WRITE THIS FUNCTION
    public Vector2 _FindEmptyPlace (int objectSize) {
        bool isOccupied = false;
        int countTime = 0;
        int min = 10;
        int max = 30;
        int randomI = 0, randomJ = 0;
        int timeToIncreaseScope = 20;
        int timeToTry = 500;

        while (true) {
            isOccupied = false;
            randomI = (int) Mathf.Floor(UnityEngine.Random.Range(min, max));
            randomJ = (int) Mathf.Floor(UnityEngine.Random.Range(min, max));

            for (var e = randomI; e > randomI - objectSize; e--) {
                for (var f = randomJ; f > randomJ - objectSize; f--) {
                    if (e < 0 || f < 0 || e > 39 || f > 39) {
                        isOccupied = true;
                        f = randomJ - objectSize;
                        e = randomI - objectSize;
                    }
                    else if (this.data[e, f] != -1) {
                        if (countTime % timeToIncreaseScope == 0) {
                            if (min > 0) min -= 1;
                            if (max < 39) max += 1;
                        }
                        isOccupied = true;
                        f = randomJ - objectSize;
                        e = randomI - objectSize;
                    }
                    countTime += 1;

                    // If we cannot find empty slot by random
                    if (countTime > timeToTry) {
                        var startLimit = objectSize;

                        // We'll do it the hard way
                        for (var r = startLimit; r <= 39; r++) {
                            for (var s = startLimit; s <= 39; s++) {
                                isOccupied = false;
                                for (var q = r; q > r - objectSize; q--) {
                                    for (var p = s; p > s - objectSize; p--) {
                                        if (this.data[q, p] != -1) {
                                            isOccupied = true;
                                            q = r - objectSize;
                                            p = s - objectSize;
                                        }
                                    }
                                }
                                if (isOccupied == false) {
                                    return new Vector2(r, s);
                                }
                            }
                        }
                        return new Vector2(-1, -1);
                    }
                }
            }
           if (isOccupied == false) break;
        }
        return new Vector2(randomI, randomJ);

    }

    // Check if a specific place has enough cell for an object with currentId & size
    public bool _IsValidPlace (Vector2 place, int currentId, int size) {
        for (float i = place.x; i > place.x - size; i--) {
            for (float j = place.y; j > place.y - size; j--) {
                if (data[(int) i, (int) j] != -1 && data[(int) i, (int) j] != currentId) {
                    return false;
                }
            }
        }
        return true;
    }

    // Save map data to local storage
    public void _SaveToStorage () {
        BinaryFormatter formatter = new BinaryFormatter ();
        FileStream stream = new FileStream (path, FileMode.Create);
        formatter.Serialize (stream, MapDataSerialized._MapDataToSerialized (this));
        stream.Close ();
    }

    // Load map data from local storage
    public void _LoadFromStorage (string _path = null) {
        string chosenPath = path;
        if (_path != null) {
            chosenPath = _path;
        }

        if (File.Exists (chosenPath)) {
            //Debug.Log (chosenPath);
            BinaryFormatter formatter = new BinaryFormatter ();
            FileStream stream = new FileStream (chosenPath, FileMode.Open);
            if (stream.Length ==0 ) { // If want to reset data, change this to > 0
                stream.Close ();
                this._InitFirstTime ();
                //this._SaveToStorage ();
            } else {
                MapDataSerialized mapDataSerialized = formatter.Deserialize (stream) as MapDataSerialized;
                MapDataSerialized._SerializedToMapData (mapDataSerialized);

                //this._AddMapObject(BuildingFactory.BuildingType.MINE, 1, new Vector2(70, 25));
                stream.Close ();
            }
        } else {
            this._InitFirstTime ();
            this._SaveToStorage ();
        }
    }

    // Load map objects gui

    public void _LoadObjectsGUI () {
        

        for (int i = 0; i < objects.Count; i++) {
            BuildingFactory.BuildingType objectType = (BuildingFactory.BuildingType) objects[i].type;
            objects[i].spriteUI = BuildingFactory.instance.createBuilding (objectType, CameraManager.instance.transform);
            objects[i]._LoadGUI ();

            if(i==1)
            {
                objects[i].spriteUI.transform.Find("Structure").GetComponentInChildren<SpriteRenderer>().sprite = null;
            }
        }
    }

    public void _ResetAll() {

        objects.Clear();
        selectedObject = null;

        for (int i = 0; i < Number.MAP_SIZE; i++) {
            for (int j = 0; j < Number.MAP_SIZE; j++) {
                data[i, j] = -1;
            }
        }
    }
    
    public void _ChangeBuildingState(Building.BuildingState state)
    {
        foreach(MapObject obj in objects)
        {
            obj._ChangeState(state);
        }
    }
}