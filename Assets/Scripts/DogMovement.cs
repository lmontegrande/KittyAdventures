using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : Character
{
  public Transform[] patrolPoints;
  public float currentSpeed = 0.01f;
  public float patrolTimeWait = 2f;
  public float alarmTimeWait = 3f;
  public float sight = 1.2f;
  public float force;
  int currentPoint;
  bool isPaused = false;
  bool isCoroutineStarted = false;
  string dogState = "patrol";  // bark, kill
  bool hasSeen;
  Animator anim;

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
  }

  void Update ()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.localScale.x * Vector2.right, sight, 1 << LayerMask.NameToLayer("Player"));

    if (hit.collider != null && hit.collider.name == "Quad")
    {
      hasSeen = true;

      if (dogState == "patrol")
      {
        alarmTimeWait = 3f;
        anim.SetBool("Running", true);
        dogState = "bark";
      }
      else if (dogState == "bark")
      {
        if (alarmTimeWait > 0f) // wait for 3s
        {
          alarmTimeWait -= Time.deltaTime;
          dogState = "bark";
          // GetComponent<Rigidbody2D>().AddForce(Vector3.up * force * 30 + (hit.collider.transform.position-transform.position) * force);
          StopCoroutine ("Patrol");
          StartCoroutine("Bark");
        }
        else
        {
          dogState = "kill";
        }
      }
      else if (dogState == "kill")
      {
        Debug.Log("Kill the cat!");
        alarmTimeWait = 3f;
        dogState = "patrol";
        anim.SetBool("Running", true);
        StopCoroutine ("Bark");
        StopCoroutine ("Patrol");
        StartCoroutine("Kill", hit);
      }
    }

    // out of the dog's sight
    if (hit.collider == null && hasSeen && dogState != "check")
    {
      dogState = "check";
      anim.SetBool("Running", true);
      StopCoroutine ("Bark");
      StartCoroutine ("Check");
    }

    if (dogState == "check" && hit.collider == null)
    {
      anim.SetBool("Running", false);
      alarmTimeWait = 3f;
      StopCoroutine ("Bark");
      StopCoroutine("Kill");
      StartCoroutine("Check");
      StopCoroutine("Patrol");
    }

    if (hit.collider == null && dogState == "patrol")
    {
      StopCoroutine ("Bark");
      StopCoroutine("Kill");
      StopCoroutine("Check");
      StartCoroutine("Patrol");
      anim.SetBool("Running", true);
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

  IEnumerator Bark ()
  {
    anim.SetBool("Running", false);
    yield return new WaitForSeconds(patrolTimeWait);
    yield return null;
  }

  IEnumerator Check ()
  {
    yield return new WaitForSeconds(1);
    transform.localScale = -transform.localScale;
    yield return new WaitForSeconds(1);
    transform.localScale = -transform.localScale;
    yield return new WaitForSeconds(1);
    dogState = "patrol";
  }

  IEnumerator Kill (RaycastHit2D col)
  {
    // moving distance calculation problem
    transform.position = Vector2.MoveTowards(transform.position, new Vector2(col.point.x, transform.position.y), 15 * Time.deltaTime);
    yield return new WaitForSeconds(1);
    OnCollisionEnter2D(col);
    yield return null;
  }

  void OnCollisionEnter2D(RaycastHit2D col)
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
