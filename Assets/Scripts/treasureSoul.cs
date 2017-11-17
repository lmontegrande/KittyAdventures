using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class treasureSoul : MonoBehaviour {

    //GameObject chest = GameObject.FindGameObjectWithTag("Chest");
    public GameObject treasure;

	// Use this for initialization
	void Start () {
        treasure.SetActive(true);
        treasure.SetActive(false);
        treasure.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<openChest>().chestIsOpen == true)
        {
            treasure.SetActive(true);
            treasure.GetComponent<SpriteRenderer>().enabled = true;

        }
	}
    
 }

