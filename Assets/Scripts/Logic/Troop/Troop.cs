using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Troop {
    public int id;
    public int type;
    public int state;
    public int direction;
    public int indexSprite;
    public Vector2 vectorDirection;
    public Vector2 destinationPos;
    public GameObject go;

    public List<Vector3> pathToTarget;
    public MapObject target;

    public Troop (int _id, bool isRender = true) {
        // Game object
        if (isRender) {
            go = new GameObject (String.TROOP_PREFIX + _id);
            go.AddComponent<SpriteRenderer> ();
            go.transform.localScale = new Vector3 (Number.TROOP_SCALE_SIZE, Number.TROOP_SCALE_SIZE, 1);
            go.GetComponent<SpriteRenderer> ().sortingOrder = 99;
            go.transform.parent = CameraManager.instance.troopObjects.transform;

            var shadow = new GameObject ("Shadow");
            shadow.AddComponent<SpriteRenderer> ().sprite = TroopManager.instance.troopShadow;
            shadow.transform.parent = go.transform;
            shadow.GetComponent<SpriteRenderer> ().sortingOrder = 98;
        }

        // Logic number
        direction = Number.DIRECTION_E;
        state = Number.ARM_IDLE_STATE;
        indexSprite = 0;
        id = _id;

        // Path to run
        pathToTarget = new List<Vector3> ();
    }

    public void _SetPosition (Vector2 cellPosition) {
        destinationPos = cellPosition;

        if (GamePlayController.instance != null) {
            go.transform.position = GamePlayController.instance._CellToWorld (destinationPos);
        } else if (BattleController.instance != null) {
            var pos = BattleController.instance._CellToWorld (destinationPos);
            go.transform.position = new Vector2 (pos.x + Random.Range (-0.1f, 0.1f), pos.y + +Random.Range (-0.1f, 0.1f));
        }
    }

    public Vector2 _GetPosition () {
        return go.transform.position;
    }

    public virtual void _Update () { }

    public void _SetState (int _state) {
        state = _state;
        indexSprite = 0;
    }

    public void _SetTarget (Vector2 _destinationPos) {
        this.destinationPos = _destinationPos;
        this._SetDirection (-1);
    }

    public void _SetDirection (int fixedDirection) {
        if (fixedDirection != -1) {
            this.direction = fixedDirection;
        } else {
            float na = this.destinationPos.x - go.transform.position.x;
            float nb = this.destinationPos.y - go.transform.position.y;

            // Calculate vector direction
            float nc = Mathf.Max (Mathf.Abs (na), Mathf.Abs (nb));
            this.vectorDirection = new Vector2 (na / nc, nb / nc);

            // Calculate direction
            this.direction = (((int) Mathf.Round (Mathf.Atan2 (nb, na) / (2 * 3.14F / 8))) + 8) % 8;
        }
    }

    public void _AddPath (List<Vector2> _path) {
        _StopRunning ();

        for (int i = _path.Count - 1; i >= 0; i--) {
            if (GamePlayController.instance != null) {
                this.pathToTarget.Add (GamePlayController.instance._CellToWorld (_path[i]));
            } else {
                this.pathToTarget.Add (BattleController.instance._CellToWorld (_path[i]));
            }
        }
    }

    public void _RunWithPath () {
        if (pathToTarget.Count > 0) {
            state = Number.ARM_RUN_STATE;
            this._SetTarget (this.pathToTarget[0]);
            this.pathToTarget.RemoveAt (0);
        } else {
            this._SetState (Number.ARM_ATTACK_STATE);
        }
    }

    public void _StopRunning () {
        pathToTarget.Clear ();
        this._SetState (Number.ARM_IDLE_STATE);
    }

    public void _FindTarget () {

        var srcPos = BattleController.instance._WorldToCell (go.transform.position);

        int minDistanceObject = -1;
        float minDistance = 10000f;

        for (int i = 0; i < MapData.instance.objects.Count; i++) {
            if (MapData.instance.objects[i].state != Number.MAP_DEAD_STATE) {
                if (minDistance > Vector2.Distance (srcPos, MapData.instance.objects[i].place)) {
                    minDistanceObject = i;
                    minDistance = Vector2.Distance (srcPos, MapData.instance.objects[i].place);
                }
            }
        }

        if (minDistanceObject != -1) {
            target = MapData.instance.objects[minDistanceObject];
            int midSize = target.size / 2;
            var desPos = new Vector2Int ((int) target.place.x - midSize, (int) target.place.y - midSize);

            _StopRunning ();
            var path = PathFinding._PathFinding (PathFinding.nodeData[(int) srcPos.x, (int) srcPos.y], PathFinding.nodeData[desPos.x, desPos.y]);

            _AddPath (path);
            _RunWithPath ();
        } else {
            _StopRunning ();
        }
    }

    public void _updateVitality(int damage) {

        Debug.Log("TROOP WILL BE DAMAGED");

    }
}