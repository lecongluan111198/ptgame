using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MapObject {
    public int id { get; set; }
    public int level { get; set; }
    public int type { get; set; }//not save this
    public int state { get; set; }
    public int finishBuildingTime { get; set; }
    public Vector2 place { get; set; }
    public int size { get; set; }
    // public Building spriteUI { get; set; }
    public GameObject spriteUI { get; set; }
    public Building building = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake () {
        _LoadBuilding ();
    }

    protected void _LoadBuilding () {
        building = spriteUI.GetComponent<Building> ();
    }
    public MapObject () { }

    // Load sprites gui
    public void _LoadGUI () {
        if (this.building == null)
            _LoadBuilding ();
        //Debug.Log (spriteUI);
        //Debug.Log (this.building);
        this.building._LoadGUI (place, id);
    }

    public void _UpdatePlace () {
        Vector2 newPlace = GamePlayController.instance._WorldToCell (this.building._GetPosition ());
        bool isValidPlace = MapData.instance._IsValidPlace (newPlace, id, size);
        if (isValidPlace == true) {
            // Remove old place
            for (float i = place.x; i > place.x - size; i--) {
                for (float j = place.y; j > place.y - size; j--) {
                    MapData.instance.data[(int) i, (int) j] = -1;
                }
            }
            // Assign new place
            place = GamePlayController.instance._WorldToCell (this.building._GetPosition ());
            for (float i = place.x; i > place.x - size; i--) {
                for (float j = place.y; j > place.y - size; j--) {
                    MapData.instance.data[(int) i, (int) j] = id;
                }
            }
        } else {
            // Restore position
            // sprites.transform.position = GamePlayController.instance._CellToWorld(place);
            building._UpdatePlace (place);
        }
    }

    public void _SetSelectedState () {
        building._SetSelectedState ();
    }
    public void _SetDeselectedState () {
        building._SetDeselectedState ();
    }

    public void _SetPosition (Vector3 position) {
        this.building._SetPosition (position);

        if (MapData.instance._IsValidPlace (GamePlayController.instance._WorldToCell (position), id, size)) {
            this.building._SetGreenState ();
        } else {
            this.building._SetRedState ();
        }
    }

    public void _updateVitality(float damage) {
        
        building._updateVitality(damage);

        // After building explode
        if (building._IsExploxive()) {
            state = Number.MAP_DEAD_STATE;

            // Find new target for troops
            if (BattleController.instance != null) {
                BattleController.instance._FindTargetAgainForAllTroops();
            }
        }
    }

    public void _ChangeState(Building.BuildingState state)
    {
        this.state = (int) state;
        building._ChangeState(state);
    }
}