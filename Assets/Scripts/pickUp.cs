﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour {

    public int soulAmnt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(other);
            soulAmnt++;
        }
    }
}
