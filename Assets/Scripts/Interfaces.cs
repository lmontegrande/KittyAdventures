using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtable {
    void GetHurt(int damage);
}

public interface IKillable
{
    void Die();
}