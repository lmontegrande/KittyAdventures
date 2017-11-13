using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
  public float moveForce;
  public float jumpForce;
	// Use this for initialization
	void Start ()
  {

	}

	// Update is called once per frame
	void Update ()
  {
    float h = Input.GetAxis("Player1_Horizontal") * moveForce;
    GetComponent<Rigidbody2D>().AddForce(new Vector2(h, 0));

    if (Input.GetKeyDown (KeyCode.Space))
    {
      GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
    }
	}
}
