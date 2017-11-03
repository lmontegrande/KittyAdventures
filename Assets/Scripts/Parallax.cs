using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public float moveFactor = .25f;
    public Vector3 imageOffset;

    private Camera camera;
    private Vector3 cameraStartingPosition;
    private float cameraStartingOrthoSize;

    public void Start()
    {
        camera = Camera.main;
        cameraStartingPosition = camera.transform.position;
    }

    public void Update()
    {
        Vector3 target = camera.transform.position - camera.transform.position * moveFactor;
        transform.position = new Vector3(target.x, cameraStartingPosition.y, transform.position.z) + imageOffset;
    }
}
