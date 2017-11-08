using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameState
    {
        Start, Pause, Play
    }

    public static GameManager instance;

    private GameState _currentGameState;

    public void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void Pause()
    {
        
    }
}
