using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour {
    public GameObject Green, Red, ArrowMove, Fence, Structure, ButtonHolder, healthBar, Junk, collectBar, goldIcon;
    protected int size;
    // [SerializeField]
    protected float health = 100;
    protected float currentHealth;
    protected BuildingFactory.BuildingType typeObject;
    protected BuildingState state;

    public enum BuildingState {
        NORMAL = 1,
        DEAD = 2,
        ATTACK = 3
    }

    private float id;
    private float totalTimeCollect = 60f;
    private float collectTimeLeft;
    private float timeToRefreshCollectBar = 0.0F;
    private float intervalRefreshCollectBar = 1.0F;

    void Awake () {
        this.currentHealth = this.health;
        state = BuildingState.NORMAL;
    }

    // Start is called before the first frame update
    protected virtual void Start () {

        var timeToCollect = PlayerPrefs.GetFloat ("User_" + UserInfo.Instance.id + "_Building_" + id, -1);

        var epochStart = new System.DateTime (2020, 1, 1, 1, 1, 1, System.DateTimeKind.Utc);
        var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

        if (timeToCollect == -1 || timeToCollect < timestamp) {
            collectTimeLeft = 0;
            _ShowCoinCollect ();
        } else {
            collectTimeLeft = timeToCollect - Mathf.FloorToInt ((float) timestamp);
        }
    }

    // Update is called once per frame
    protected virtual void Update () {

        if (collectTimeLeft > 0 && Time.time > timeToRefreshCollectBar) {
            timeToRefreshCollectBar += intervalRefreshCollectBar;
            collectTimeLeft--;
            _ShowCollectBar ();
            if (collectTimeLeft == 0) {
                _ShowCoinCollect ();
            }
        }

    }

    public void _LoadGUI (Vector2 place, int _id) {
        _UpdatePlace (place);

        id = _id;
        _ShowCollectBar ();
    }

    public Vector3 _GetPosition () {
        return transform.position;
    }

    public void _SetPosition (Vector3 pos) {
        transform.position = pos;
    }

    public void _UpdatePlace (Vector2 place) {

        Vector3 world = Vector3.zero;
        if (GamePlayController.instance != null) {
            world = GamePlayController.instance._CellToWorld (place);
        } else if (BattleController.instance != null) {
            world = BattleController.instance._CellToWorld (place);
        }

        Vector3 pos = new Vector3 (world.x, world.y, transform.position.z);
        _SetPosition (pos);
    }

    public void _SetSelectedState () {
        Green.SetActive (true);
        ArrowMove.SetActive (true);
        // Structure.SetActive (false);
        //ButtonHolder.SetActive (true);
    }

    public void _SetDeselectedState () {
        Green.SetActive (false);
        ArrowMove.SetActive (false);
        // Structure.SetActive (true);
        //ButtonHolder.SetActive (false);
    }

    public void _SetRedState () {
        Green.SetActive (false);
        Red.SetActive (true);
    }

    public void _SetGreenState () {
        Green.SetActive (true);
        Red.SetActive (false);
    }

    public void _updateVitality (float down) {
        if (state == BuildingState.ATTACK) {
            this.currentHealth -= down;
            var newScale = this.healthBar.transform.localScale;
            newScale.x = Mathf.Clamp01 (this.currentHealth / this.health);

            this.healthBar.transform.localScale = newScale;
            if (this.currentHealth <= 0) {
                //start explosive
                _SetExplosive ();
                state = BuildingState.DEAD;
            }
        }
    }

    public void _SetExplosive () {
        Junk.SetActive (true);
        Structure.SetActive (false);
    }

    public bool _IsExploxive () {
        return currentHealth <= 0;
    }

    public void _ChangeState (BuildingState state) {
        this.state = state;
        if (state == BuildingState.ATTACK) {
            healthBar.SetActive (true);
        } else {
            healthBar.SetActive (false);
        }
    }
    public void _ShowCollectBar () {
        if (collectBar != null) {
            var newScale = collectBar.transform.localScale;
            newScale.x = 1 - Mathf.Clamp01 (collectTimeLeft / totalTimeCollect);
            collectBar.transform.localScale = newScale;
        }
    }

    public void _ShowCoinCollect () {
        if (collectBar != null) {
            collectBar.transform.parent.gameObject.SetActive (false);
            goldIcon.SetActive (true);
        }
    }

    public void _CollectCoin () {
        if (collectBar != null && collectTimeLeft == 0) {

            // Check capactity
            if (UserInfo.Instance.Coin == UserInfo._amountCoin) {
                NotifyBoardUI.Instance._Active ("Kho vàng của bạn đã đầy", "Hãy yên tâm số vàng trong mỏ vẫn được giữ nguyên nhé. Hãy thu hoạch khi còn kho vàng còn chỗ trống");
                return;
            }

            collectBar.transform.parent.gameObject.SetActive (true);
            goldIcon.SetActive (false);

            collectTimeLeft = totalTimeCollect;
            timeToRefreshCollectBar = Time.time;

            var epochStart = new System.DateTime (2020, 1, 1, 1, 1, 1, System.DateTimeKind.Utc);
            var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            var saved = Mathf.FloorToInt ((float) timestamp) + totalTimeCollect;

            PlayerPrefs.SetFloat ("User_" + UserInfo.Instance.id + "_Building_" + id, saved);

            // Add money
            UserInfo.Instance.increaseMoney (1000);
            GameObject.FindObjectOfType<MainUIController> ()._loadAllUserInfo ();
        }
    }
}