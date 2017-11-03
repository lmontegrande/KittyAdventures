using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour {

  public Transform[] patrolPoints;
  public float currentSpeed = 0.01f;
  public float timeWait = 2f;
  public float sight = 1.2f;
  public float force;
  int currentPoint;

  Animator anim;

  void Start ()
  {
    anim = GetComponent<Animator> ();
    StartCoroutine ("Patrol");
    anim.SetBool("Running", true);
  }

  void Update ()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.localScale.x * Vector2.right, sight + transform.position.x, 9);

    Debug.Log(hit.collider);

    if (hit.collider != null && hit.collider.tag == "Quad")
    {
      GetComponent<Rigidbody2D> ().AddForce(Vector3.up * force * 100 + (hit.collider.transform.position-transform.position) * force);
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
        yield return new WaitForSeconds(timeWait);
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

  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.tag == "Player")
    {
      // Destroy (col.gameObject);
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;

    Gizmos.DrawLine(transform.position, transform.position + transform.localScale.x * Vector3.right * -sight);
  }
}
