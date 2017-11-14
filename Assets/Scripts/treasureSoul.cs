using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class treasureSoul : MonoBehaviour {

    //GameObject chest = GameObject.FindGameObjectWithTag("Chest");
    public GameObject treasure;
    public int soulAmnt;

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
