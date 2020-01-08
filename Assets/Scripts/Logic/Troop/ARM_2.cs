using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARM_2 : Troop
{
    public ARM_2(int _id, bool _isRender = true) : base(_id, _isRender) { type = 2; }

    public static float SPEED = 0.85F;

    public override void _Update()
    {
        // If troop is running, change its position
        if (state == Number.ARM_RUN_STATE) {
            if (Vector2.Distance(destinationPos, go.transform.position) < 0.75F) {
                _RunWithPath();
            }
            else {
                go.transform.Translate(vectorDirection * Time.deltaTime * ARM_1.SPEED);
            }
        }

        // Change sprite
        if (++indexSprite == Number.ARM_2_ANIM_FRAME[state]) {
            indexSprite = 0;
        }

        // Show arrow
        if (indexSprite == 9 && state == Number.ARM_ATTACK_STATE) {
            BulletManager.instance._ShowTroopArrow(go.transform.position, destinationPos, target);
        }

        go.GetComponent<SpriteRenderer>().sprite = TroopManager.instance.ARM_SPRITES[Number.ARM_2_INDEX][state][direction][indexSprite];
    }
}
