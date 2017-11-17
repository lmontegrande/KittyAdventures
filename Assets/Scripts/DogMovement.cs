using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : Character
{
  public Transform[] patrolPoints;
  public float currentSpeed = 0.01f;
  public float patrolTimeWait = 2f;
  float alarmTimeWait = 2f;
  float checkTimeWait = 3f;
  public float sight = 1.2f;
  int currentPoint;
  bool isPaused = false;
  string dogState = "patrol";  // bark, kill
  bool firstBark = true;
  Animator anim;
  private AudioSource source;
  public AudioClip warn;
  public AudioClip kill;

  public override void Pause()
  {
    isPaused = true;
  }

  public override void UnPause()
  {
    isPaused = false;
  }

  public override void Awake()
  {
    base.Awake();
    source = GetComponent<AudioSource>();
  }

  void Start ()
  {
    dogState = "patrol";
    anim = GetComponent<Animator> ();
    StartCoroutine ("Patrol");
    anim.SetBool("Running", true);
  }

  void Update ()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.localScale.x * Vector2.right, sight, 1 << LayerMask.NameToLayer("Players") | 1 << LayerMask.NameToLayer("Enviroment"));
    if (dogState == "patrol")
    {
      checkTimeWait = 3f;
    }

    // if dog has seen the player
    //if (hit.collider != null && (hit.collider.name == "RightCollider" || hit.collider.name == "LeftCollider" || hit.collider.name == "FootCollider"))
    if (hit.collider != null && hit.collider.tag == "CatBody")
    {
      if (dogState == "patrol")
      {
        alarmTimeWait = 2f;
        anim.SetBool("Running", false);
        dogState = "bark";
        firstBark = true;
      }
      else if (dogState == "bark")
      {
        if (firstBark)
        {
          source.PlayOneShot(warn, 1f);
          firstBark = false;
        }

        StopCoroutine("Patrol");

        if (alarmTimeWait == 2f)
        {
          alarmTimeWait -= Time.deltaTime;
        }
        if (alarmTimeWait < 2 && alarmTimeWait > 0)
        {
          alarmTimeWait -= Time.deltaTime;
        }
        else
        {
          source.PlayOneShot(kill, 1f);
          alarmTimeWait = 2f;
          dogState = "kill";
        }
      }
      else // suppose to check
      {
        alarmTimeWait = 2f;
        dogState = "bark";
      }
    }

    // player out of the dog's sight
    if ((hit.collider == null || hit.collider.tag == "Floor") && dogState == "bark")
    {
      dogState = "check";
    }

    if (dogState == "check")
    {
      firstBark = true;

      if (checkTimeWait <= 2.5f && checkTimeWait >= 1.5f)
      {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      }
      if (checkTimeWait < 1.5f && checkTimeWait >= 0.5f)
      {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      }

      if (checkTimeWait <= 0f)
      {
        checkTimeWait = 3f;
        dogState = "patrol";
        anim.SetBool("Running", true);
        StartCoroutine("Patrol");
      }

      // player gets into sight when check
      if (hit.collider != null && hit.collider.tag == "CatBody")
      {
        checkTimeWait = 3f;
        dogState = "bark";
      }
      else
      {
        checkTimeWait -= Time.deltaTime;
      }
    }

    if (dogState == "kill")
    {
      transform.position = Vector2.MoveTowards(transform.position, new Vector2(Mathf.Abs(hit.distance) * transform.localScale.x, transform.position.y), currentSpeed * 2);

      anim.SetBool("Running", true);
      dogState = "patrol";
      firstBark = true;
      Kill(hit);
      StartCoroutine("Patrol");
    }
  }

  IEnumerator Patrol ()
  {
    while (true)
    {
      if (transform.position.x == patrolPoints[currentPoint].position.x)
      {
        currentPoint++;
        anim.SetBool("Running", false);
        yield return new WaitForSeconds(patrolTimeWait);
        anim.SetBool("Running", true);
      }

      if (currentPoint >= patrolPoints.Length)
      {
        currentPoint = 0;
      }

      transform.position = Vector2.MoveTowards(transform.position, new Vector2(patrolPoints[currentPoint].position.x, transform.position.y), currentSpeed);

      if (transform.position.x > patrolPoints[currentPoint].position.x)
      {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      }
      else if (transform.position.x < patrolPoints[currentPoint].position.x)
      {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      }
      yield return null;
    }
  }

  void Kill (RaycastHit2D col)
  {
    if (col.collider == null)
      return;
    // moving distance calculation problem
    transform.position = Vector2.MoveTowards(transform.position, new Vector2(col.point.x, transform.position.y), 15 * Time.deltaTime);
    DestroyPlayer(col);
  }

  void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.name == "Cat" || col.gameObject.tag == "CatBody")
    {
      GameObject.Find("Cat").GetComponent<Cat>().Die();
    }
  }

  void DestroyPlayer(RaycastHit2D col)
  {
    GameObject.Find("Cat").GetComponent<Cat>().Die();
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + transform.localScale.x * Vector3.right * -sight);
  }
}
