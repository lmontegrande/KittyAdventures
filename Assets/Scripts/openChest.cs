using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openChest : MonoBehaviour {

    public Sprite openSprite;
    public bool chestIsOpen = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(chestIsOpen == true)
        {

        }
		
	}

    //left as player for now, change when key added
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetComponent<SpriteRenderer>().sprite = openSprite;
            chestIsOpen = true;

        }
    }
}
