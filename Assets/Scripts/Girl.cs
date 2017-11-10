using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : PlayerControlledCharacter
{
    bool isTouchingCat = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Cat cat = collision.GetComponent<Cat>();
        if (cat != null)
        {
            cat.isBeingHeld = true;
        }
    }

    public override void LadderEnter(bool isEnter)
    {
        isTouchingladder = isEnter;
    }

    protected override void ReleaseSkill()
    {
        
    }

    protected override void UseSkill()
    {
        
    }
}
