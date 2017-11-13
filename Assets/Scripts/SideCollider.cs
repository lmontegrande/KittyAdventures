using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour {

    public delegate void OnSideEnterHandle();
    public delegate void OnSideExitHandle();
    public delegate void OnLedgeEnterHandle(GameObject ledgeTile);

    public OnSideEnterHandle OnSideEnter;
    public OnSideExitHandle OnSideExit;
    public OnLedgeEnterHandle OnLedgeEnter;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ledge" && OnLedgeEnter != null)
        {
            OnLedgeEnter(collision.gameObject);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
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
