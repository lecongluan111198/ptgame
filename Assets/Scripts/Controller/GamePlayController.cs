using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Logic.Network;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Assets.Scripts.Logic.Audio;

public class GamePlayController : MonoBehaviour {
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    GameObject[] ButtonsInfo;

    // private Dictionary<Vector3Int,
    private int[, ] map;

    static public GamePlayController instance = null;

    // Start is called before the first frame update
    void Start () {
        Debug.Log("gameplaycontroller");
        if(instance == null)
            GamePlayController.instance = this;
        // Initalize map data
        if (MapData.instance == null)
        {
            MapData.instance = new MapData();
            StartCoroutine(APIManager.Instance._DowloadMap(UserInfo.Instance.id, MapData.path, (rp) =>
            {
                if (rp)
                {
                    MapData.instance._LoadFromStorage();
                    MapData.instance._LoadObjectsGUI();
                    MapData.instance._ChangeBuildingState(Building.BuildingState.NORMAL);
                }
            }));
        }
        else
        {

        }

        _testAPI();
    }

    private void _testAPI()
    {
        //StartCoroutine(APIManager.Instance._GetUserTrooper(UserInfo.Instance.id));
        //StartCoroutine(APIManager.Instance._GetUserBuilding(UserInfo.Instance.id));
        //StartCoroutine(APIManager.Instance._IncreaseUserCoin(UserInfo.Instance.id, 100));
        //StartCoroutine(APIManager.Instance._DecreaseUserTrooper(UserInfo._GetUserId(), new Dictionary<string, int>()));
        //StartCoroutine(APIManager.Instance._IncreaseUserTrooper(UserInfo._GetUserId(), new Dictionary<string, int>()));
    }

    private int indexOfButton(GameObject button)
    {
        for(int i=0;i< ButtonsInfo.Length; i++)
        {
            if(ButtonsInfo[i]== button)
            {
                return i;
            }
        }
        return -1;
    }

    // Update is called once per frame
    void Update () {

        // Touch down event
        if (Input.GetMouseButtonDown (0)) {
            //TODO: check if mouse down on button
            var currentSelection = EventSystem.current.currentSelectedGameObject;
            if(indexOfButton(currentSelection)!=-1)
            {
                return;
            }

            // NotifyBoardUI.Instance._Active("Title", "Content ....");

                GameObject MainUI = GameObject.Find("MainLayoutController");

            // Get cell clicked
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mousePos);
            Vector2 cell = _WorldToCell(mousePos);
            //Debug.Log(cell);
            // Firstly, deselect selected object if neccessary
            if (MapData.instance.selectedObject != null) {
                MapData.instance.selectedObject._SetDeselectedState();
                MapData.instance.selectedObject = null;
                MainUI.GetComponent<MainUIController>()._hideBuidlingAciont();

            }

            // Then, select the new object (if clicked on it)
            if (cell.x >= 0 && cell.y >= 0 && cell.x <= Number.MAP_SIZE - 1 && cell.y <= Number.MAP_SIZE - 1) {
                int cellValue = MapData.instance.data[(int)cell.x, (int)cell.y];
                if (cellValue != -1) {
                    MapData.instance.selectedObject = MapData.instance.objects[cellValue];
                    MapData.instance.selectedObject._SetSelectedState();
                    MainUI.GetComponent<MainUIController>()._showBuildingAction();
                }
            }
            else {
                MapData.instance.selectedObject = null;
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
        //Debug.Log(cell);
        cell.x = cell.x * -1 - 5;
        cell.y = cell.y * -1 - 8;
        Vector3Int pos = new Vector3Int ();
        pos.x = (int) cell.x;
        pos.y = (int) cell.y;
        pos.z = 0;
        //Debug.Log(tilemap);
        return tilemap.CellToWorld (pos);
    }

    public void saveMap()
    {
        MapData.instance._SaveToStorage();
        StartCoroutine(APIManager.Instance._UploadMap(UserInfo.Instance.id, MapData.path));
    }

    public void StartBattle() {
        SceneManager.LoadScene("BattleScene");
        //MapData.instance._ChangeBuildingState(Building.BuildingState.ATTACK);
    }

    public void _UpdateBuilding()
    {
        GameObject.FindObjectOfType<AudioManager>().playSound(SoundEnum.Click);
        if (MapData.instance.selectedObject != null)
        {

            Debug.Log("haha");
        }
        else
        {
            Debug.Log("huhu");
        } 
    }

    public void _CollectGold() {
        if (MapData.instance.selectedObject != null) {
            MapData.instance.selectedObject.building._CollectCoin();
        }
    }
}