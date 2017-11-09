using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : PlayerControlledCharacter
{
    public override void LadderEnter(bool isEnter)
    {
        isTouchingladder = isEnter;
    }

    protected override void UseSkill()
    {
        throw new System.NotImplementedException();
    }
}
