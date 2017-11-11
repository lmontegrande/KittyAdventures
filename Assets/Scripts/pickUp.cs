using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour {

    public delegate void OnCollectHandler();
    public OnCollectHandler OnCollect;

    public AudioClip collectAudioClip;
    public Sprite collectedSprite;
    public GameObject container;
    public float followDelay = .25f;
    public float followFarRange = 5f;

    private GameObject followTarget;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (OnCollect != null)
                OnCollect.Invoke();
            Destroy(GetComponent<CircleCollider2D>());

            //followTarget = other.gameObject;
            //followTarget = GameObject.Find("Cat");
            //StartCoroutine(LerpFollow());

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
            Vector3 deltaVector = followTarget.transform.position - container.transform.position;
            float x = deltaVector.magnitude / followFarRange;
            container.transform.position = Vector3.Lerp(container.transform.position, followTarget.transform.position, x);
            yield return new WaitForSeconds(followDelay);
        }
    }
}
