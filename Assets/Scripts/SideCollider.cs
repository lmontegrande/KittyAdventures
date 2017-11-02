using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour {

    public delegate void OnSideEnterHandle();
    public delegate void OnSideExitHandle();

    public OnSideEnterHandle OnSideEnter;
    public OnSideExitHandle OnSideExit;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" && OnSideEnter != null)
        {
            OnSideEnter.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall" && OnSideExit != null)
        {
            OnSideExit.Invoke();
        }
    }
}
