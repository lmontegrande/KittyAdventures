﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSwitch : MonoBehaviour {

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
            Debug.Log(collision);
            //collision.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Door").SetActive(false);

        }
    }
}