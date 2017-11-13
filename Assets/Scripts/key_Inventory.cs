using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class key_Inventory : MonoBehaviour {

    public bool keyInv;

    public Image gotKey;
	// Use this for initialization
	void Start () {
        gotKey.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gotKey.enabled = true;
            Destroy(GetComponent<CircleCollider2D>());
            GetComponent<SpriteRenderer>().sprite = null;
            keyInv = true;
        }
    }
}
