using System.Collections;
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
    public float teleportEffectLinger = 1.5f;
    public GameObject teleportEffect;

    private LineRenderer _lineRenderer;

    public void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = numRopePoints;
        cat.GetComponent<Cat>().usePull += HandlePhysics;
        cat.GetComponent<Cat>().releasePull += LetGo;
    }

    public void Update()
    {
        DrawRope();
        HandleInput();
    }

    public void LateUpdate()
    {
        //HandlePhysics(); 
    }

    private void DrawRope()
    {
        Vector2 deltaVector = cat.transform.position - girl.transform.position;

        for (int x=0; x<numRopePoints; x++)
        {
            Vector2 currentDeltaVector = (deltaVector / (numRopePoints-1)) * x;

            //Random.Range(0f,1f), Mathf.Sin((((float)x /numRopePoints) * 360) + 180), Mathf.Abs(x - (numRopePoints/2)) + Time.deltaTime
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

        Vector2 catVelocity = cat.GetComponent<Rigidbody2D>().velocity;
        Vector2 girlVelocity = girl.GetComponent<Rigidbody2D>().velocity;

        Vector2 catToGirlVector = girl.transform.position - cat.transform.position;
        Vector2 girlToCatVector = cat.transform.position - girl.transform.position;

        //if (deltaVector.magnitude < distanceAllowed)
        //{
        //    cat.GetComponent<Cat>().tetherIsPulling = false;
        //    girl.GetComponent<Girl>().tetherIsPulling = false;
        //    return;
        //}
        //cat.GetComponent<Cat>().tetherIsPulling = true;
        //girl.GetComponent<Girl>().tetherIsPulling = true;

        //if (catToGirlVector.y >= distanceAllowed/2)
        //    cat.GetComponent<PlayerControlledCharacter>().isBeingPulledUp = true; 
        //else
        //    cat.GetComponent<PlayerControlledCharacter>().isBeingPulledUp = false;

        //if (girlToCatVector.y >= distanceAllowed/2)
        //    girl.GetComponent<PlayerControlledCharacter>().isBeingPulledUp = true;
        //else
        //    girl.GetComponent<PlayerControlledCharacter>().isBeingPulledUp = false;

        Vector2 applyCatVelocity = Vector2.zero;
        Vector2 applyGirlVelocity = Vector2.zero;
        applyCatVelocity = (Vector2)(catToGirlVector) * rubberBandingFloat;
        applyGirlVelocity = (Vector2)(girlToCatVector) * rubberBandingFloat;
        

        //cat.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyCatVelocity, Vector3.zero, distanceAllowed / deltaVector.magnitude);
        girl.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyGirlVelocity, Vector3.zero, distanceAllowed / deltaVector.magnitude);
        girl.GetComponent<PlayerControlledCharacter>().isBeingPulled = true;
    }

    private void LetGo()
    {
        girl.GetComponent<PlayerControlledCharacter>().isBeingPulled = false;
        girl.transform.rotation = Quaternion.identity;
        //girl.transform.position += Vector3.up;
    }
    
    private void HandleInput()
    {
        if (Input.GetButtonDown("Swap"))
        {
            Vector3 temp;
            temp = girl.transform.position + Vector3.up * yCatSwapOffset;
            girl.transform.position = cat.transform.position + Vector3.up * yGirlSwapOffset;
            cat.transform.position = temp;
            Destroy(Instantiate(teleportEffect, girl.transform.position, Quaternion.identity), teleportEffectLinger);
            Destroy(Instantiate(teleportEffect, cat.transform.position, Quaternion.identity), teleportEffectLinger);
        }
    }
}
