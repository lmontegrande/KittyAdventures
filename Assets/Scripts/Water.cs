using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {       
            collision.GetComponent<PlayerControlledCharacter>().Die();
        }
    }
}
