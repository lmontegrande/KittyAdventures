﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : Character
{
  public Transform[] patrolPoints;
  public float currentSpeed = 0.01f;
  public float patrolTimeWait = 2f;
  float alarmTimeWait = 2f;
  float checkTimeWait = 5f;
  public float sight = 1.2f;
  public float force;
  int currentPoint;
  bool isPaused = false;
  string ghostState = "patrol";
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
    ghostState = "patrol";
    anim = GetComponent<Animator> ();
    StartCoroutine ("Patrol");
    // anim.SetBool("Running", true);
	}

	// Update is called once per frame
	void Update ()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.localScale.x * Vector2.right, sight, 1 << LayerMask.NameToLayer("Players"));

    // Debug.Log(hit.collider.gameObject.tag);

    if (ghostState == "patrol")
    {
      checkTimeWait = 5f;
    }

    // if dog has seen the player
    if (hit.collider != null && (hit.collider.name == "RightCollider" || hit.collider.name == "LeftCollider" || hit.collider.name == "FootCollider"))
    {
      // Debug.Log("see the girl");
      if (ghostState == "patrol")
      {
        alarmTimeWait = 2f;
        // anim.SetBool("Running", false);
        ghostState = "bark";
        firstBark = true;
      }
      else if (ghostState == "bark")
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
          ghostState = "kill";
        }
      }
      else // suppose to check
      {
        alarmTimeWait = 2f;
        ghostState = "bark";
      }
    }

    // player out of the dog's sight
    if (hit.collider == null && ghostState == "bark")
    {
      ghostState = "check";
    }

    if (ghostState == "check")
    {
      firstBark = true;

      if (checkTimeWait <= 4f && checkTimeWait > 2f)
      {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      }
      if (checkTimeWait <= 2f && checkTimeWait > 0f)
      {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      }

      if (checkTimeWait <= 0.1f)
      {
        checkTimeWait = 5f;
        ghostState = "patrol";
        // anim.SetBool("Running", true);
        StartCoroutine("Patrol");
      }

      // player gets into sight when check
      if (hit.collider != null && (hit.collider.name == "RightCollider" || hit.collider.name == "LeftCollider" || hit.collider.name == "FootCollider"))
      {
        checkTimeWait = 5f;
        ghostState = "bark";
      }
      checkTimeWait -= Time.deltaTime;
    }

    if (ghostState == "kill")
    {
      // anim.SetBool("Running", true);
      ghostState = "patrol";
      firstBark = true;
      Kill(hit);
    }

	}

  IEnumerator Patrol ()
  {
    while (true)
    {
      if (transform.position.x == patrolPoints[currentPoint].position.x)
      {
        currentPoint++;
        // anim.SetBool("Running", false);
        yield return new WaitForSeconds(patrolTimeWait);
        // anim.SetBool("Running", true);
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
    if (col.gameObject.name == "Girl" || col.gameObject.tag == "GirlBody")
    {
      GameObject.Find("Cat").GetComponent<Cat>().Die();
    }
  }

  void DestroyPlayer(RaycastHit2D col)
  {
    GameObject.Find("Girl").GetComponent<Girl>().Die();
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + transform.localScale.x * Vector3.right * -sight);
  }
}