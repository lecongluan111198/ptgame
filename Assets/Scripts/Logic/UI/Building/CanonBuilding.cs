using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBuilding : Building {
    private bool isAttack = false;
    private Troop target;
    private int[] dx = {-1, -1, -1, 0, 1, 1, 1, 0 };
    private int[] dy = {-1, 0, 1, 1, 1, 0, -1, -1 };
    int maxX;
    int minX;
    int maxY;
    int minY;

    private string[] direction = { "sw", "w", "nw", "n", "ne", "e", "se", "s" };

    // Start is called before the first frame update
    // void Start () {

    // }

    // Update is called once per frame
    protected override void Update () {
        if (state == BuildingState.ATTACK) {
            if (!isAttack) {
                _FindEnemy ();

            }
        }
    }

    private bool _IsInAttackArea (Vector2 desCell) {
        if (desCell.x >= minX && desCell.x <= maxX && desCell.y >= minY && desCell.y <= maxY)
            return true;
        return false;
    }

    private void _FindEnemy () {
        Vector2 place = BattleController.instance._WorldToCell (_GetPosition ());
        Vector2 pos;
        List<Troop> troops = TroopManager.instance.listTroop;
        int x = (int) place.x;
        int y = (int) place.y;
        maxX = x + 9;
        minX = x - 9;
        maxY = y + 9;
        minY = y - 9;

        foreach (Troop t in troops) {
            pos = BattleController.instance._WorldToCell (t._GetPosition ());
            if (_IsInAttackArea (pos)) {
                isAttack = true;
                target = t;
                Debug.Log (t._GetPosition () + " " + pos);
                StartCoroutine (_Attack ());
                //TODO: start rotate()
                break;
            }
        }
    }

    private IEnumerator _Attack () {
        while (true) {
            try {
                _Rotate ();
                BulletManager.instance._ShowBuildingBullet (_GetPosition (), target._GetPosition (), target);
                // target._updateVitality(10);
                //TODO: if troop die then isAttrack = false; break;
                Vector2 pos = BattleController.instance._WorldToCell (target._GetPosition ());
                if (state == BuildingState.DEAD || !_IsInAttackArea (pos) || (target == null && !ReferenceEquals (target, null))) {
                    isAttack = false;
                    break;
                }

            } catch (NullReferenceException e) {
                Debug.Log (e);
                isAttack = false;
                break;
            }
            yield return new WaitForSecondsRealtime (1);

        }
        yield return null;
    }

    private void _Rotate () {
        Vector2 place = _GetPosition ();
        Vector2 targetPlace = target._GetPosition ();
        float minAngle = 360;
        int direct = 0;
        for (int i = 0; i < 8; i++) {
            Vector2 v1 = new Vector2 (dx[i] - place.x, dy[i] - place.y);
            Vector2 v2 = new Vector2 (targetPlace.x - place.x, targetPlace.y - place.y);
            float angle = Vector2.Angle (v1, v2);
            // Debug.Log (place + " " + targetPlace + " " + v1 + " " + v2 + " " + direction[i] + " " + angle);
            if (angle < minAngle) {
                minAngle = angle;
                direct = i;
            }
        }

        Structure.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (String.BUILDING_CANON_PATH + "run_" + direction[direct]);

    }

    private double _DistanceFromPointToLine (Vector2 from, Vector2 to, Vector2 p) {
        double t = Mathf.Abs ((to.y - from.y) * p.x - (to.x - from.x) * p.y - from.x * to.y + from.y * to.x);
        double m = Mathf.Sqrt ((to.y - from.y) * (to.y - from.y) + (from.x - to.x) * (from.x - to.x));
        return t / m;
    }
}