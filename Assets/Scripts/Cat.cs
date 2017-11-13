using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : PlayerControlledCharacter
{

    public delegate void UsePullHandler();
    public delegate void ReleasePullHandler();
    public UsePullHandler usePull;
    public ReleasePullHandler releasePull;

    public override void LadderEnter(bool isEnter)
    {
        // Cat can't climb ladders
    }

    protected override void UseSkill()
    {
        if (usePull != null)
            usePull.Invoke();
    }

    protected override void ReleaseSkill()
    {
        if (releasePull != null)
            releasePull.Invoke();
    }
}
