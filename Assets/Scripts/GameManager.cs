using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public List<Character> characters;

    public void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void Pause()
    {
        foreach (Character character in characters)
            character.Pause();
    }

    public void UnPause()
    {
        foreach (Character character in characters)
            character.UnPause();
    }
}
