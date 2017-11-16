using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : PlayerControlledCharacter
{

    public GameObject tornado;

    public delegate void UsePullHandler();
    public delegate void ReleasePullHandler();
    public UsePullHandler usePull;
    public ReleasePullHandler releasePull;

    public override void LadderEnter(bool isEnter)
    {
        // Cat can't climb ladders
    }

    public override void Start()
    {
        base.Start();
        tornado.SetActive(false);
    }

    public void AimTornado(Vector2 direction)
    {
        tornado.transform.up = direction;
    }

    protected override void UseSkill()
    {
        if (usePull != null)
            usePull.Invoke();
        tornado.SetActive(true);
    }

    protected override void ReleaseSkill()
    {
        if (releasePull != null)
            releasePull.Invoke();
        tornado.SetActive(false);
    }
}
