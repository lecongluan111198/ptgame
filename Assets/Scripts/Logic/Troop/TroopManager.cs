using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TroopManager
{
    public static TroopManager instance;
    public List<List<List<Sprite[]>>> ARM_SPRITES;
    public List<Troop> listTroop;
    public Sprite troopShadow;

    public TroopManager()
    {
        // Initialize
        listTroop       = new List<Troop>();
        ARM_SPRITES     = new List<List<List<Sprite[]>>>();
        troopShadow     = Resources.Load<Sprite>(String.TROOP_PATH + String.TROOP_SHADOW);
        
        string[] ARMS   = new string[] { "ARM_1", "ARM_2" };
        string[] STATES = new string[] { "IDLE", "RUN", "ATTACK" };
        string[] DIRECT = new string[] { "E", "NE", "N", "NW", "W", "SW", "S", "SE" };

        // Loading sprites
        for (int k = 0; k < ARMS.Length; k++) {
            List<List<Sprite[]>> ARM = new List<List<Sprite[]>>();
            for (int i = 0; i < STATES.Length; i++) {
                List<Sprite[]> STATE = new List<Sprite[]>();
                for (int j = 0; j < DIRECT.Length; j++) {
                    STATE.Add(TroopManager._LoadSpritesheet(ARMS[k], STATES[i], DIRECT[j]));
                }
                ARM.Add(STATE);
            }
            ARM_SPRITES.Add(ARM);
        }
    }

    private static Sprite[] _LoadSpritesheet(string arm, string state, string direction)
    {
        string filePath = String.TROOP_PATH + arm + "/" + state + "_" + direction;
        return Resources.LoadAll<Sprite>(filePath);
    }

    public void _Update()
    {
        for (int i = 0; i < listTroop.Count; i++) {
            if (listTroop[i].go != null) {
                listTroop[i]._Update();
            }
        }
    }
}
