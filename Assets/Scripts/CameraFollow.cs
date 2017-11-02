using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float lerpFactor = .5f;
    public float minOrthographicSize = 5f;
    public float maxOrthographicSize = 10f;

    private GameObject[] players;
    private Vector3 startingPosition;
    private Camera _camera;

    public void Start()
    {
        startingPosition = transform.position; 
    }

	public void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        _camera = GetComponent<Camera>();

        Vector3 targetPosition = new Vector3();
        int aliveSize = 0;

        foreach(GameObject player in players)
        {
            if (!player.GetComponent<PlayerControlledCharacter>().isDead)
            {
                targetPosition += player.transform.position;
                aliveSize++;
            }
        }

        if (aliveSize <= 0) return;

        targetPosition = Vector3.Lerp(gameObject.transform.position, targetPosition / aliveSize, lerpFactor);

        if (players.Length >= 2)
        {
            Vector2 deltaVector = Vector2.zero;
            foreach (GameObject player in players)
            {
                if ((targetPosition - player.transform.position).magnitude >= deltaVector.magnitude)
                {
                    deltaVector = player.transform.position - targetPosition;
                }
            }

            // Fix Camera Scaling for multiple characters
            Debug.Log(deltaVector + " " + deltaVector.magnitude);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, (Mathf.Clamp(deltaVector.magnitude, minOrthographicSize, Mathf.Infinity)), Time.deltaTime);
        } else {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, minOrthographicSize, Time.deltaTime);
        }
        gameObject.transform.position = new Vector3(targetPosition.x, Mathf.Clamp(targetPosition.y, startingPosition.y, 1000), gameObject.transform.position.z);
    }
}
