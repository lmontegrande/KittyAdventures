using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IPausable
{
    public abstract void Pause();
    public abstract void UnPause();

    public virtual void Start()
    {
        GameManager.instance.characters.Add(this);
    }
}
