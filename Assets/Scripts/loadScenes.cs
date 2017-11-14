using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class loadScenes : MonoBehaviour {

    public int neededSouls;
    public string nextLevel;
    private int currentAmount;
    public GameObject soulShards;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //gets the amount of souls from the pick up counter
        currentAmount = soulShards.GetComponent<pickUp>().soulCounter;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the door is being collided with the player and the amount of souls is correct
        if (collision.gameObject.CompareTag("Player") && neededSouls == currentAmount)
        {
            //loads next level
            loadLevel(nextLevel);
            Debug.Log(nextLevel);
            Debug.Log("I can reach!");
        }
    }
    //loads the level
    public void loadLevel (string levelName) { 
        SceneManager.LoadScene(levelName);
    }


}
