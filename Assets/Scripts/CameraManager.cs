using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Logic.Network;

public class CameraManager : MonoBehaviour
{
	public static CameraManager instance;
	public SpriteRenderer sandSprite;
	public SpriteMask gameObjects;
	public SpriteMask troopObjects;
	public SpriteMask bulletObjects;
	public Text userName;

	void Awake () {
		instance = this;
	}

	// Variables for zoom and pan
	private float screenRatio;
	private float minZoom, maxZoom;
	private float minX, maxX, minY, maxY;
	private Vector3 panVelocity;
	private Vector3 previousPanPoint;
	private float previousPinchDistance;
    private float timeToRefreshTroop = 0.0F;
	private float intervalRefreshTroop = 0.075F;
    private float timeToRefreshBullet = 0.0F;
	private float intervalRefreshBullet = 0.025F;
    private float timeToRefreshBuilding = 0.0F;
	private float intervalRefreshBuilding = 2.0F;
    private float timeToRefreshBuildingBullet = 0.0F;
	private float intervalRefreshBuildingBullet = 0.005F;
	private bool isMovedMap = false;

	// Calculate boundaries for panning
	void CalcBoundaries()
	{
		float height = Camera.main.orthographicSize * 2.0F;
		float width = height * screenRatio;

		maxX = (sandSprite.bounds.size.x - width) / 2.0F;
		minX = -maxX;

		maxY = (sandSprite.bounds.size.y - height) / 2.0F;
		minY = -maxY;
	}

	void Start()
	{
		// Calculate apporiate zooming boundaries
		screenRatio = (float) Screen.width / Screen.height;
		maxZoom = sandSprite.bounds.size.x / screenRatio / 2;
		minZoom = 1.0F;
		Camera.main.orthographicSize = maxZoom;

		// Calculate apporiate panning boundaries
		CalcBoundaries();

		// Display UI
		if (userName) {
			userName.text = UserInfo.Instance.userName;
		}

		if (TroopManager.instance == null) TroopManager.instance = new TroopManager();
		if (BulletManager.instance == null) BulletManager.instance = new BulletManager();

		// Get list troop
		StartCoroutine(APIManager.Instance._GetUserTrooper(UserInfo.Instance.id, (rp) => {
			if (rp) {
				if (GamePlayController.instance != null) {
					// GamePlayController.instance._UpdateTotalTroop();
				}
				else if (BattleController.instance != null) {
					for (int i = 0; i < TroopManager.instance.listTroop.Count; i++) {
						BattleController.instance.totalTroop[TroopManager.instance.listTroop[i].type - 1]++;
					}
					BattleController.instance._UpdateTotalTroop();
					TroopManager.instance.listTroop.Clear();
				}
			}
		}));
	}

    void Update()
    {
		this.UpdateScenePan();
		this.UpdateSceneZoom();
		this.UpdateBattle();

		if (Input.GetMouseButtonUp(0) == true)
		{
			if (isMovedMap == true) {
				isMovedMap = false;
			}
			else {
        		this.UpdateGroundTap();
			}
		}
    }

	// TODO: MODIFY THIS
	private void UpdateBattle()
	{
		// Update building
		if (Time.time > timeToRefreshBuilding) {
			timeToRefreshBuilding += intervalRefreshBuilding;

			// ĐÂY LÀ VÍ DỤ OBJECT[0] BẮN TROOP[0]
			// if (TroopManager.instance.listTroop.Count > 0) {
			// 	var go = MapData.instance.objects[0].spriteUI;
			// 	var destinationPos = TroopManager.instance.listTroop[0].go.transform.position;
            // 	BulletManager.instance._ShowBuildingBullet(go.transform.position, destinationPos, TroopManager.instance.listTroop[0]);
			// }

			// LUẬN TODO: Làm hàm MapData.instance._Update(); nhằm update các nhà phòng thủ để bắn
		}

		// Update troop
		if (Time.time > timeToRefreshTroop) {
            timeToRefreshTroop += intervalRefreshTroop;
            TroopManager.instance._Update();
        }
		
		// Update troop bullet
		if (Time.time > timeToRefreshBullet) {
            timeToRefreshBullet += intervalRefreshBullet;
            BulletManager.instance._UpdateTroopBullet();
		}
		
		// Update building bullet
		if (Time.time > timeToRefreshBuildingBullet) {
            timeToRefreshBuildingBullet += intervalRefreshBuildingBullet;
            BulletManager.instance._UpdateBuildingBullet();
		}
	}

	// TODO: MODIFY THIS 
	private void UpdateGroundTap()
	{
		if (BattleController.isFighting && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			BattleController.instance._PutTroop();
		}
	}
	
	private void UpdateScenePan ()
	{
		if (Input.GetMouseButtonDown (0) == true) {
			this.previousPanPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}

		if (Input.GetMouseButton(0) == true)
		{
			// Move map
			if (MapData.instance.selectedObject == null) {
				Vector3 delta = this.previousPanPoint - Camera.main.ScreenToWorldPoint(Input.mousePosition);
				transform.position += delta;
				Vector3 pos = transform.position;
				pos.x = Mathf.Clamp(pos.x, minX, maxX);
				pos.y = Mathf.Clamp(pos.y, minY, maxY);
				transform.position = pos;

				isMovedMap = isMovedMap || delta.x != 0 || delta.y != 0;
			}
			// Move object
			else {
				
				// Prevent click button
				if (Input.mousePosition.y < 150 && Input.mousePosition.x > 400 && Input.mousePosition.x < 888) {
					return;
				}

				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            	Vector2 cell = GamePlayController.instance._WorldToCell(mousePos);
				if (cell.x < MapData.instance.selectedObject.size - 1) {
					cell.x = MapData.instance.selectedObject.size - 1;
				}
				if (cell.y < MapData.instance.selectedObject.size - 1) {
					cell.y = MapData.instance.selectedObject.size - 1;
				}
				if (cell.x > Number.MAP_SIZE - 1) {
					cell.x = Number.MAP_SIZE - 1;
				}
				if (cell.y > Number.MAP_SIZE - 1) {
					cell.y = Number.MAP_SIZE - 1;
				}
				Vector3 fixedPos = GamePlayController.instance._CellToWorld(cell);
				MapData.instance.selectedObject._SetPosition(fixedPos);
			}
		}
		else if (Input.GetMouseButtonUp(0) == true) {
			if (MapData.instance.selectedObject != null) {
				MapData.instance.selectedObject._UpdatePlace();

				// Save map after move object
				GamePlayController.instance.saveMap();
			}
		}
	}

	private void UpdateSceneZoom ()
	{
		float newScale = Camera.main.orthographicSize;

		// In editor
		newScale = newScale - Input.GetAxis ("Mouse ScrollWheel");

		// In mobile
		if (Input.touchCount == 2)
		{
			Touch currTouch0 = Input.GetTouch (0);
			Touch currTouch1 = Input.GetTouch (1);

			Vector2 prevTouch0 = currTouch0.position - currTouch0.deltaPosition;
			Vector2 prevTouch1 = currTouch1.position - currTouch1.deltaPosition;

			float prevMagnitude = (prevTouch1 - prevTouch0).magnitude;
			float currMagnitude = (currTouch1.position - currTouch0.position).magnitude;

			float difference = (currMagnitude - prevMagnitude) * 0.1F;
			newScale = newScale - difference;
		}

		// If there is no change at all
		if (newScale == Camera.main.orthographicSize) return;

		// Adjust inside boundaries
		newScale = Mathf.Clamp (newScale, minZoom, maxZoom);

		// Bouncing effect
		if (newScale < minZoom + 1.0F)
		{
			newScale = Mathf.Lerp(newScale, minZoom + 1.0F, Time.deltaTime * 3);
		}
		else if (newScale > maxZoom - 1.0F)
		{
			newScale = Mathf.Lerp(newScale, maxZoom - 1.0F, Time.deltaTime * 3);
		}

		// Zoom
		if (Camera.main.orthographicSize != newScale)
		{
			Camera.main.orthographicSize = newScale;
			
			// Adjust inside boundaries
			CalcBoundaries();
			Vector3 pos = transform.position;
			pos.x = Mathf.Clamp(pos.x, minX, maxX);
			pos.y = Mathf.Clamp(pos.y, minY, maxY);
			transform.position = pos;
		}
	}
}