using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour {

    public delegate void OnLeaveGroundHandle();
    public delegate void OnLandGroundHandle();

    public OnLeaveGroundHandle OnLeaveGround;
    public OnLandGroundHandle OnLandGround;

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Floor" && OnLandGround != null)
        {
            OnLandGround.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Floor" && OnLeaveGround != null)
        {
            OnLeaveGround.Invoke();
        }
    }
}
