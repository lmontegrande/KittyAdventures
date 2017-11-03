﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulRope : MonoBehaviour {

    public GameObject cat, girl;
    public float distanceAllowed = 2f;
    public float rubberBandingFloat = 1f;
    public float yCatSwapOffset = 1f;
    public float yGirlSwapOffset = 1f;
    public Color ropeColor = Color.black;
    public int numRopePoints = 10;

    private LineRenderer _lineRenderer;

    public void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = numRopePoints;
    }

    public void Update()
    {
        DrawRope();
        HandleInput();
    }

    public void LateUpdate()
    {
        HandlePhysics(); 
    }

    private void DrawRope()
    {
        Vector2 deltaVector = cat.transform.position - girl.transform.position;

        for (int x=0; x<numRopePoints; x++)
        {
            Vector2 currentDeltaVector = (deltaVector / numRopePoints) * x;

            //Random.Range(0f,1f)
            Vector2 newPosition = Vector2.Lerp(_lineRenderer.GetPosition(x), (Vector2)girl.transform.position + currentDeltaVector, Random.Range(0f, 1f)); // Play around with this for interesting behavior

            _lineRenderer.SetPosition(x, newPosition);
        }

        Color lineColor = Color.Lerp(Color.clear, ropeColor, deltaVector.magnitude / distanceAllowed);
        //_lineRenderer.SetPosition(0, girl.transform.position);
        //_lineRenderer.SetPosition(1, cat.transform.position);
        _lineRenderer.startColor = lineColor;
        _lineRenderer.endColor = lineColor;
    }

    private void HandlePhysics()
    {
        Vector2 deltaVector = cat.transform.position - girl.transform.position;
        float scalar = 2f; 

        if (deltaVector.magnitude < distanceAllowed)
        {
            cat.GetComponent<Cat>().tetherIsPulling = false;
            girl.GetComponent<Girl>().tetherIsPulling = false;
            return;
        }

        cat.GetComponent<Cat>().tetherIsPulling = true;
        girl.GetComponent<Girl>().tetherIsPulling = true;

        Vector2 catVelocity = cat.GetComponent<Rigidbody2D>().velocity;
        Vector2 girlVelocity = girl.GetComponent<Rigidbody2D>().velocity;

        Vector2 applyCatVelocity = (Vector2)(girl.transform.position - cat.transform.position) * rubberBandingFloat;
        Vector2 applyGirlVelocity = (Vector2)(cat.transform.position - girl.transform.position) * rubberBandingFloat;

        Debug.DrawLine(cat.transform.position, cat.transform.position + (Vector3)applyCatVelocity);
        Debug.DrawLine(girl.transform.position, girl.transform.position + (Vector3)applyGirlVelocity);

        cat.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyCatVelocity, Vector3.zero, distanceAllowed / deltaVector.magnitude);
        girl.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyGirlVelocity, Vector3.zero, distanceAllowed / deltaVector.magnitude);
    }
    
    private void HandleInput()
    {
        if (Input.GetButtonDown("Skill"))
        {
            Vector3 temp;
            temp = girl.transform.position + Vector3.up * yCatSwapOffset;
            girl.transform.position = cat.transform.position + Vector3.up * yGirlSwapOffset;
            cat.transform.position = temp;
        }
    }
}
