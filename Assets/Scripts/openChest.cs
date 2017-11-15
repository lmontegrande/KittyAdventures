using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class openChest : MonoBehaviour {

    public Sprite openSprite;
    public bool chestIsOpen;
    public GameObject keyAsset;
    public Image gotKey;

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
        if (collision.gameObject.CompareTag("Player") && keyAsset.GetComponent<key_Inventory>().keyInv == true)
        {
            //turn off image
            gotKey.enabled = false;
            //get rid of key in the inventory
            keyAsset.GetComponent<key_Inventory>().keyInv = false;
            //sets the chest to open
            transform.GetComponent<SpriteRenderer>().sprite = openSprite;
            //say that the chest is now open
            chestIsOpen = true;

        }
    }
}
