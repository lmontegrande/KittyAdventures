using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasureSoul : MonoBehaviour {

    //GameObject chest = GameObject.FindGameObjectWithTag("Chest");
    public GameObject treasure;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<openChest>().chestIsOpen == true)
        {
            treasure.SetActive(true);
        }
	}
}
