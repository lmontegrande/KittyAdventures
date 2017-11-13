using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openChest : MonoBehaviour {

    public Sprite openSprite;
    public bool chestIsOpen;

    //public key_Inventory keyInInv;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //left as player for now, change when key added && collision.GetComponent<key_Inventory>().keyInv == true
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if (collision.gameObject.CompareTag("Player") && GetComponent<key_Inventory>().keyInv == true)
        {
            transform.GetComponent<SpriteRenderer>().sprite = openSprite;
            Debug.Log("reached");
            chestIsOpen = true;

        }
    }
}
