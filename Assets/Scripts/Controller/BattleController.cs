using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic.Network;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Button[] topButtons;

    [SerializeField]
    private Button[] botButtons;

    [SerializeField]
    private Sprite[] disableSprites;

    [SerializeField]
    private Sprite[] visibleSprites;

    static public BattleController instance = null;
    static public bool isFighting = false;

    public Text rivalName;
    public GameObject prepareUI;

    private int chosenTroop = -1;
    public int[] totalTroop;
    private int[] listTroopLeft;

    void Start () {

        BattleController.instance = this;
        totalTroop = new int[] { 0, 0, 0, 0 };
        listTroopLeft = new int[] { 0, 0, 0, 0 };

        if (MapData.instance == null) {
            MapData.instance = new MapData ();
        }

        // Get random rival
        StartCoroutine (APIManager.Instance._GetRandomUser (UserInfo.Instance.id, (name, rivalid) => {
            if (rivalid != -1) {
                rivalName.text = name;
                StartCoroutine (APIManager.Instance._DowloadMap (rivalid, MapData.rivalPath, (rp) => {
                    if (rp) {
                        MapData.instance._ResetAll ();
                        MapData.instance._LoadFromStorage (MapData.rivalPath);
                        MapData.instance._LoadObjectsGUI ();
                        Debug.Log("battle scene");
                        MapData.instance._ChangeBuildingState (Building.BuildingState.ATTACK);
                    }
                }));
            }
        }));
    }

    public void _Exit () {

    }

    public void _StartBattle () {
        prepareUI.SetActive (false);
        BattleController.isFighting = true;
    }

    public void _PutTroop () {

        if (BattleController.isFighting == false) {
            return;
        }

        if (chosenTroop >= 0 && chosenTroop <= listTroopLeft.Length - 1) {
            if (listTroopLeft[chosenTroop] > 0) {
                Troop troop = null;
                if (chosenTroop == 0) {
                    troop = new ARM_1 (1);
                } else if (chosenTroop == 1) {
                    troop = new ARM_2 (1);
                }
                TroopManager.instance.listTroop.Add (troop);
                troop._SetPosition (_WorldToCell (Camera.main.ScreenToWorldPoint (Input.mousePosition)));
                troop._FindTarget ();

                // Update number of left troop
                listTroopLeft[chosenTroop]--;
                _UpdateChosenTroop ();
            }
        }
    }

    public void _FindTargetAgainForAllTroops () {
        for (int i = 0; i < TroopManager.instance.listTroop.Count; i++) {
            TroopManager.instance.listTroop[i]._FindTarget ();
        }
    }

    public void _IncreaseTroop (int index) {
        if (totalTroop[index] > 0) {
            totalTroop[index]--;
            listTroopLeft[index]++;
        }
        _UpdateTotalTroop ();
        _UpdateChosenTroop ();
    }

    public void _DecreaseTroop (int index) {

        int realIndex = index;
        int.TryParse (botButtons[index].tag, out realIndex);

        // If is in battle, mark chosen troop
        if (BattleController.isFighting) {
            chosenTroop = realIndex;
            return;
        }

        if (botButtons[index].tag == "4") {
            return;
        }

        if (listTroopLeft[realIndex] > 0) {
            listTroopLeft[realIndex]--;
            totalTroop[realIndex]++;
        }
        _UpdateTotalTroop ();
        _UpdateChosenTroop ();
    }

    public void _UpdateChosenTroop () {
        int currentIndex = 0;
        for (int i = 0; i < listTroopLeft.Length; i++) {
            if (listTroopLeft[i] > 0) {
                botButtons[currentIndex].GetComponent<Image> ().sprite = visibleSprites[i];
                botButtons[currentIndex].GetComponentInChildren<Text> ().text = "x" + listTroopLeft[i];
                botButtons[currentIndex].GetComponentInChildren<Text> ().enabled = true;
                botButtons[currentIndex].tag = i.ToString ();
                currentIndex++;
            }
        }
        for (int i = currentIndex; i < listTroopLeft.Length; i++) {
            botButtons[i].GetComponent<Image> ().sprite = visibleSprites[4];
            botButtons[i].GetComponentInChildren<Text> ().enabled = false;
            botButtons[currentIndex].tag = "4";
        }
    }

    public void _UpdateTotalTroop () {
        for (int i = 0; i < totalTroop.Length; i++) {
            topButtons[i].GetComponentInChildren<Text> ().text = "x" + totalTroop[i];
            if (totalTroop[i] == 0) {
                topButtons[i].GetComponent<Image> ().sprite = disableSprites[i];
            } else {
                topButtons[i].GetComponent<Image> ().sprite = visibleSprites[i];
            }
        }
    }

    public Vector2 _WorldToCell (Vector3 world) {
        Vector3Int gridPos = tilemap.WorldToCell (world);
        gridPos.x = (gridPos.x + 5) * -1;
        gridPos.y = (gridPos.y + 8) * -1;
        return new Vector2 (gridPos.x, gridPos.y);
    }

    public Vector3 _CellToWorld (Vector2 cell) {
        cell.x = cell.x * -1 - 5;
        cell.y = cell.y * -1 - 8;
        Vector3Int pos = new Vector3Int ();
        pos.x = (int) cell.x;
        pos.y = (int) cell.y;
        pos.z = 0;
        return tilemap.CellToWorld (pos);
    }

    // public void _UpdateAttackForState () {
    //     List<MapObject> objects = MapData.instance.objects;
    //     foreach(MapObject obj in  objects){
    //         obj._ChangeState(Building.BuildingState.ATTACK);
    //     }
    // }
}