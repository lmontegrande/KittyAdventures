using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUp : MonoBehaviour {

    public delegate void OnCollectHandler();
    public OnCollectHandler OnCollect;

    public AudioClip collectAudioClip;
    public GameObject container;
    public float followDelay = .25f;
    public float moveSpeed = 1f;
    public float targetYOffset = 1f;

    private GameObject followTarget;

    //For UI
    //Checks if anything has been picked up
    public bool ifCollected = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (OnCollect != null)
                OnCollect.Invoke();
            Destroy(GetComponent<CircleCollider2D>());

            ifCollected = true;
            followTarget = other.gameObject;
            followTarget = GameObject.Find("SnowDoor");
            if(followTarget != null)
                StartCoroutine(LerpFollow());

            GetComponent<ParticleSystem>().Play();
            GetComponent<AudioSource>().PlayOneShot(collectAudioClip);
            GetComponent<SpriteRenderer>().sprite = null;
            GetComponent<Animator>().SetBool("isCollected", true);
            
        }
    }
 
    private IEnumerator LerpFollow()
    {
        while (true)
        {
            container.transform.position = Vector3.MoveTowards(container.transform.position, followTarget.transform.position + (Vector3.up * targetYOffset), moveSpeed);
            yield return new WaitForSeconds(followDelay);
        }
    }


}
