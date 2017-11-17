using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSwitch : MonoBehaviour {

    public GameObject door;
    public Sprite downSwitch; 
    public Sprite open_Door;
    public AudioClip buttonAudioClip;
    public GameObject trail;
    public float lineSpeed = .25f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if(collision.gameObject.CompareTag("Player"))
        {
            //collision.GetComponent<BoxCollider2D>().enabled = false;
            //this.gameObject.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(buttonAudioClip);
            transform.GetComponent<SpriteRenderer>().sprite = downSwitch;
            StartCoroutine(SendLineRenderer());
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public IEnumerator SendLineRenderer()
    {
        Vector3 startVector = transform.position;
        Vector3 endVector = door.transform.position;
        GameObject trailInstance = Instantiate(trail, transform.position, Quaternion.identity);
        float timer = 0f;
        while (timer <= lineSpeed)
        {
            timer += Time.deltaTime;
            trailInstance.transform.position = Vector3.Lerp(startVector, endVector, timer/lineSpeed);
            yield return null;
        }
        Destroy(trailInstance, trailInstance.GetComponent<TrailRenderer>().time);
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<SpriteRenderer>().sprite = open_Door;
    }
}
