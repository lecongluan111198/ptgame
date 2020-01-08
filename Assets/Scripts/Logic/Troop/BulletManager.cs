using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Bullet
{
    public bool isUsing;
    public Vector3 destination;
    public GameObject go;
    public object target;
}

public class BulletManager
{
    public static BulletManager instance;

    public List<Bullet> buildingBullets;
    public List<Bullet> troopArrows;
    public Sprite arrowSprite;
    public Sprite rockSprite;

    public BulletManager()  {
        buildingBullets = new List<Bullet>();
        troopArrows = new List<Bullet>();
        arrowSprite = Resources.Load<Sprite>(String.TROOP_PATH + String.ARROW);
        rockSprite = Resources.Load<Sprite>(String.BUILDING_PATH + String.ROCK);
    }

    public void _ShowTroopArrow(Vector3 srcWorld, Vector3 desWorld, MapObject _target) {
        int index = -1;

        for (int i = 0; i < troopArrows.Count; i++) {
            if (troopArrows[i].isUsing == false) {
                index = i;
                break;
            }
        }

        if (index == -1) {

            var arrow = new GameObject("Arrow_" + troopArrows.Count);
            arrow.AddComponent<SpriteRenderer>().sprite = this.arrowSprite;
            arrow.transform.parent = CameraManager.instance.bulletObjects.transform;
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 1000;
            arrow.SetActive(false);

            Bullet bullet = new Bullet()
            {
                go = arrow
            };

            index = troopArrows.Count;
            troopArrows.Add(bullet);
        }

        // Mark using
        srcWorld.y += 0.05f;
        troopArrows[index].target = _target;
        troopArrows[index].go.transform.position = srcWorld;
        troopArrows[index].destination = desWorld;
        troopArrows[index].isUsing = true;
        troopArrows[index].go.SetActive(true);

        // Set rotation
        float na = desWorld.x - srcWorld.x;
        float nb = desWorld.y - srcWorld.y;

        float angle = Mathf.Atan2(nb, na) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        
        troopArrows[index].go.transform.rotation = Quaternion.Euler (0, 0, angle);
    }

    public void _ShowBuildingBullet(Vector3 srcWorld, Vector3 desWorld, Troop _target) {
        int index = -1;

        for (int i = 0; i < buildingBullets.Count; i++) {
            if (buildingBullets[i].isUsing == false) {
                index = i;
                break;
            }
        }

        if (index == -1) {

            var rock = new GameObject("Rock_" + buildingBullets.Count);
            rock.AddComponent<SpriteRenderer>().sprite = this.rockSprite;
            rock.transform.parent = CameraManager.instance.bulletObjects.transform;
            rock.GetComponent<SpriteRenderer>().sortingOrder = 1000;
            rock.SetActive(false);

            Bullet bullet = new Bullet()
            {
                go = rock
            };

            index = buildingBullets.Count;
            buildingBullets.Add(bullet);
        }

        // Mark using
        srcWorld.y += 0.05f;
        buildingBullets[index].target = _target;
        buildingBullets[index].go.transform.position = srcWorld;
        buildingBullets[index].destination = desWorld;
        buildingBullets[index].isUsing = true;
        buildingBullets[index].go.SetActive(true);

        // Set rotation
        float na = desWorld.x - srcWorld.x;
        float nb = desWorld.y - srcWorld.y;

        float angle = Mathf.Atan2(nb, na) * Mathf.Rad2Deg;
        
        buildingBullets[index].go.transform.rotation = Quaternion.Euler (0, 0, angle);
    }

    public void _UpdateTroopBullet() {
        for (int i = 0; i < troopArrows.Count; i++) {

            if (troopArrows[i].isUsing == true) {

                var bullet = troopArrows[i].go;
                float dist_ = Vector3.Distance(troopArrows[i].destination, bullet.transform.position);

                if (dist_ < 0.25f) {
                    troopArrows[i].isUsing = false;
                    bullet.SetActive(false);
                    (troopArrows[i].target as MapObject)._updateVitality(10);
                }
                else {
                    bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, troopArrows[i].destination, 3f * Time.deltaTime);
                }
                
            }
        }
    }

    public void _UpdateBuildingBullet() {
        for (int i = 0; i < buildingBullets.Count; i++) {

            if (buildingBullets[i].isUsing == true) {

                var bullet = buildingBullets[i].go;
                float dist_ = Vector3.Distance(buildingBullets[i].destination, bullet.transform.position);

                if (dist_ < 0.1f) {
                    buildingBullets[i].isUsing = false;
                    bullet.SetActive(false);
                    (buildingBullets[i].target as Troop)._updateVitality(10);
                }
                else {
                    bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, buildingBullets[i].destination, 3f * Time.deltaTime);
                }
                
            }
        }
    }
}