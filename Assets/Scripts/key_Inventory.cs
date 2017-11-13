using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_Inventory : MonoBehaviour {

    public bool keyInv;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(GetComponent<CircleCollider2D>());
            GetComponent<SpriteRenderer>().sprite = null;
            keyInv = true;
        }
    }
}
