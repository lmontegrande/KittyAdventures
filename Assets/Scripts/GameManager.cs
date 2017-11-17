using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public List<Character> characters;
    public GameObject startingPoint;

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

    public void GameOver()
    {
        foreach(GameObject character in GameObject.FindGameObjectsWithTag("Player"))
        {
            character.GetComponent<PlayerControlledCharacter>().Respawn(startingPoint.transform.position);
        }
        //LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
