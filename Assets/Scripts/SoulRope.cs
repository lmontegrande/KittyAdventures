using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulRope : MonoBehaviour {

    public float distanceAllowed = 2f;
    public float rubberBandingFloat = 1f;
    public float yCatSwapOffset = 1f;
    public float yGirlSwapOffset = 1f;
    [Range(0, 1)]
    public float percentHideRope;
    public Color ropeColor = Color.black;
    public int numRopePoints = 10;
    public float teleportEffectLinger = 1.5f;
    public GameObject teleportEffect;
    public float letGoYOffset = 1f;

    private GameObject cat, girl;
    private LineRenderer _lineRenderer;

    public void Start()
    {
        cat = GameObject.Find("Cat");
        girl = GameObject.Find("Girl");
        if (cat == null || girl == null)
        {
            Debug.LogError("Cat or Girl not found in scene");
            return;
        }
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = numRopePoints;
        cat.GetComponent<Cat>().usePull += PullGirl;
        cat.GetComponent<Cat>().releasePull += LetGo;
    }

    public void Update()
    {
        if (cat == null || girl == null)
            return;
        DrawRope();
        HandleInput();
    }

    public void LateUpdate()
    {
        if (cat == null || girl == null)
            return;
        PullTogether();
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
        
        if (deltaVector.magnitude >= distanceAllowed*percentHideRope)
        {
            Color lineColor = Color.Lerp(Color.clear, ropeColor, (deltaVector.magnitude - (distanceAllowed * percentHideRope)) / (distanceAllowed));
            _lineRenderer.startColor = lineColor;
            _lineRenderer.endColor = lineColor;
        } else
        {
            _lineRenderer.startColor = Color.clear;
            _lineRenderer.endColor = Color.clear;
        }

        //_lineRenderer.SetPosition(0, girl.transform.position);
        //_lineRenderer.SetPosition(1, cat.transform.position);
    }

    private void PullGirl()
    {
        Vector2 deltaVector = cat.transform.position - girl.transform.position;

        Vector2 catVelocity = cat.GetComponent<Rigidbody2D>().velocity;
        Vector2 girlVelocity = girl.GetComponent<Rigidbody2D>().velocity;

        Vector2 catToGirlVector = girl.transform.position - cat.transform.position;
        Vector2 girlToCatVector = cat.transform.position - girl.transform.position;

        Vector2 applyCatVelocity = Vector2.zero;
        Vector2 applyGirlVelocity = Vector2.zero;
        applyCatVelocity = (Vector2)(catToGirlVector) * rubberBandingFloat;
        applyGirlVelocity = (Vector2)(girlToCatVector) * rubberBandingFloat;


        girl.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyGirlVelocity, Vector3.zero, 1 / deltaVector.magnitude);
        girl.GetComponent<Rigidbody2D>().velocity += applyGirlVelocity;
        girl.GetComponent<Girl>().isBeingPulled = true;
    }

    private void LetGo()
    {
        girl.GetComponent<PlayerControlledCharacter>().isBeingPulled = false;
        girl.transform.position += Vector3.up * letGoYOffset;
    }
    
    private void PullTogether()
    {
        Vector2 deltaVector = cat.transform.position - girl.transform.position;

        if (deltaVector.magnitude < distanceAllowed)
        {
            cat.GetComponent<Cat>().isBeingTethered = false;
            girl.GetComponent<Girl>().isBeingTethered = false;
            return;
        }

        cat.GetComponent<Cat>().isBeingTethered = true;
        girl.GetComponent<Girl>().isBeingTethered = true;

        Vector2 catVelocity = cat.GetComponent<Rigidbody2D>().velocity;
        Vector2 girlVelocity = girl.GetComponent<Rigidbody2D>().velocity;

        Vector2 catToGirlVector = girl.transform.position - cat.transform.position;
        Vector2 girlToCatVector = cat.transform.position - girl.transform.position;

        Vector2 applyCatVelocity = Vector2.zero;
        Vector2 applyGirlVelocity = Vector2.zero;
        applyCatVelocity = (Vector2)(catToGirlVector) * rubberBandingFloat;
        applyGirlVelocity = (Vector2)(girlToCatVector) * rubberBandingFloat;


        cat.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyCatVelocity, Vector3.zero, distanceAllowed / deltaVector.magnitude);
        girl.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Lerp(applyGirlVelocity, Vector3.zero, distanceAllowed / deltaVector.magnitude);
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Swap"))
        {
            Vector3 tempVector;
            tempVector = girl.transform.position + Vector3.up * yCatSwapOffset;
            girl.transform.position = cat.transform.position + Vector3.up * yGirlSwapOffset;
            cat.transform.position = tempVector;
            tempVector = girl.GetComponent<Rigidbody2D>().velocity;
            girl.GetComponent<Rigidbody2D>().velocity = cat.GetComponent<Rigidbody2D>().velocity;
            cat.GetComponent<Rigidbody2D>().velocity = tempVector;
            bool tempBool;
            tempBool = cat.GetComponent<Cat>().isBeingThrown;
            cat.GetComponent<Cat>().isBeingThrown = girl.GetComponent<Girl>().isBeingThrown;
            girl.GetComponent<Girl>().isBeingThrown = tempBool;
            Destroy(Instantiate(teleportEffect, girl.transform.position, Quaternion.identity), teleportEffectLinger);
            Destroy(Instantiate(teleportEffect, cat.transform.position, Quaternion.identity), teleportEffectLinger);
        }
    }
}
