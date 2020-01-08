using UnityEngine;

public class ARM_1 : Troop
{
    public ARM_1(int _id, bool _isRender = true) : base(_id, _isRender) { type = 1; }

    public static float SPEED = 0.85F;

    public override void _Update()
    {
        // If troop is running, change its position
        if (state == Number.ARM_RUN_STATE) {
            if (Vector2.Distance(destinationPos, go.transform.position) < 0.35F) {
                _RunWithPath();
            }
            else {
                go.transform.Translate(vectorDirection * Time.deltaTime * ARM_1.SPEED);
            }
        }

        // Change sprite
        if (++indexSprite == Number.ARM_1_ANIM_FRAME[state]) {
            indexSprite = 0;
        }

        // Take damage
        if (indexSprite == 9 && state == Number.ARM_ATTACK_STATE) {
            target._updateVitality(10);
        }

        go.GetComponent<SpriteRenderer>().sprite = TroopManager.instance.ARM_SPRITES[Number.ARM_1_INDEX][state][direction][indexSprite];
    }
}