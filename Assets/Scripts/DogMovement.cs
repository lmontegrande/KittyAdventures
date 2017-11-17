using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : Character
{
  public Transform[] patrolPoints;
  public float currentSpeed = 0.01f;
  public float patrolTimeWait = 2f;
  public float alarmTimeWait = 3f;
  public float checkTimeWait = 5f;
  public float sight = 1.2f;
  public float force;
  int currentPoint;
  bool isPaused = false;
  bool isCoroutineStarted = false;
  string dogState = "patrol";  // bark, kill
  bool hasSeen;
  Animator anim;
  AudioSource audio;

  public override void Pause()
  {
    isPaused = true;
  }

  public override void UnPause()
  {
    isPaused = false;
  }

  void Start ()
  {
    dogState = "patrol";
    anim = GetComponent<Animator> ();
    StartCoroutine ("Patrol");
    anim.SetBool("Running", true);
    audio = GetComponent<AudioSource>();
  }

  void Update ()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.localScale.x * Vector2.right, sight, 1 << LayerMask.NameToLayer("Players"));

    // Debug.Log(dogState);
    // Debug.Log(checkTimeWait);

    if (dogState == "patrol")
    {
      checkTimeWait = 5f;
    }

    // if dog has seen the player
    if (hit.collider != null && hit.collider.name == "Quad")
    {
      if (dogState == "patrol")
      {
        alarmTimeWait = 3f;
        anim.SetBool("Running", false);
        dogState = "bark";
      }
      else if (dogState == "bark")
      {
        StopCoroutine("Patrol");
        if (alarmTimeWait == 3f)
        {
          audio.Play();
          alarmTimeWait -= Time.deltaTime;
        }
        if (alarmTimeWait < 3 && alarmTimeWait > 0)
        {
          alarmTimeWait -= Time.deltaTime;
        }
        else
        {
          alarmTimeWait = 3f;
          dogState = "kill";
        }
      }
      else // suppose to be check
      {
        alarmTimeWait = 3f;
        dogState = "bark";
      }
    }

    // player out of the dog's sight
    if (hit.collider == null && dogState == "bark")
    {
      dogState = "check";
    }

    if (dogState == "check")
    {
      if (checkTimeWait <= 4f && checkTimeWait > 2f)
      {
        transform.localScale = new Vector3(-1, 1, 1);
      }
      if (checkTimeWait <= 2f && checkTimeWait > 0f)
      {
        transform.localScale = new Vector3(1, 1, 1);
      }

      if (checkTimeWait <= 0f)
      {
        checkTimeWait = 5f;
        dogState = "patrol";
        anim.SetBool("Running", true);
        StartCoroutine("Patrol");
      }

      // player gets into sight when check
      if (hit.collider != null && hit.collider.name == "Quad")
      {
        checkTimeWait = 5f;
        dogState = "bark";
      }
      checkTimeWait -= Time.deltaTime;
    }

    if (dogState == "kill")
    {
      Kill(hit);
      anim.SetBool("Running", true);
      dogState = "patrol";
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
        transform.localScale = Vector3.one;
      }
      else if (transform.position.x < patrolPoints[currentPoint].position.x)
      {
        transform.localScale = new Vector3(-1, 1, 1);
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

  void DestroyPlayer(RaycastHit2D col)
  {
    if (col.collider.gameObject.tag == "Player")
    {
      Destroy (col.collider.gameObject);
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + transform.localScale.x * Vector3.right * -sight);
  }
}
