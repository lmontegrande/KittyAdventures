using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IPausable
{
    public abstract void Pause();
    public abstract void UnPause();

    public virtual void Awake()
    {
        StartCoroutine(WaitForGameManager());
    }

    private IEnumerator WaitForGameManager()
    {
        GameManager gameManager = GameManager.instance;
        while (gameManager == null)
        {
            yield return null;
            gameManager = GameManager.instance;
        }
        gameManager.characters.Add(this);
    }
}
