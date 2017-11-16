using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadScenes : MonoBehaviour {

    public int neededSouls;
    public string nextLevel;
    private int currentAmount;
    public GameObject soulShardOne;
    public GameObject soulShardTwo;
    public GameObject soulShardThree;
    private int playerAmount = 0;
    public Text soulAmountText;
    // Use this for initialization
    void Start() {
       
    }

    // Update is called once per frame
    void FixedUpdate() {
        //gets the amount of souls from the pick up counter        
        trackAmount();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        //checks if the door is being collided with the player and the amount of souls is correct
        if (collision.gameObject.CompareTag("Player") && neededSouls == playerAmount)
        {
            //loads next level
            loadLevel(nextLevel);
            Debug.Log("I can reach!");
        }
    }
    //loads the level
    public void loadLevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void trackAmount()
    {
        if (soulShardOne.GetComponent<pickUp>().ifCollected == true || soulShardTwo.GetComponent<pickUp>().ifCollected == true || soulShardThree.GetComponent<pickUp>().ifCollected == true)
        {
            playerAmount = playerAmount + 1;
            soulAmountText.text = playerAmount.ToString();
            Debug.Log(playerAmount);
            soulShardOne.GetComponent<pickUp>().ifCollected = false;
            soulShardTwo.GetComponent<pickUp>().ifCollected = false;
            soulShardThree.GetComponent<pickUp>().ifCollected = false;
        }
    }

}
