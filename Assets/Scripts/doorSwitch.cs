using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSwitch : MonoBehaviour {

    public GameObject door;
    public Sprite downSwitch; 
    public Sprite open_Door;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if(collision.gameObject.CompareTag("Player"))
        {
            //collision.GetComponent<BoxCollider2D>().enabled = false;
            //this.gameObject.SetActive(true);
            transform.GetComponent<SpriteRenderer>().sprite = downSwitch;
            door.GetComponent<BoxCollider2D>().enabled = false;
            door.GetComponent<SpriteRenderer>().sprite = open_Door;
        }
    }
}
