using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : PlayerControlledCharacter
{
    public float throwStrength = 100f;
    public float grabDistance = 2f;
    public GameObject throwAimArrow;
    public float throwAimArrowOffset = 3f;
    public float flightSpan = 1f;
    public float throwOffset = 1f;

    private Cat cat;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //cat = collision.GetComponent<Cat>();
    }

    public override void Start()
    {
        base.Start();
        throwAimArrow.SetActive(false);
        cat = GameObject.Find("Cat").GetComponent<Cat>();
    }

    public override void Update()
    {
        if ((isBeingPulled && !isHolding) || isBeingTethered)
        {
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, jumpForce);
            _rigidbody2D.freezeRotation = false;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            _rigidbody2D.freezeRotation = true;
        }

        base.Update();
    }

    public override void LadderEnter(bool isEnter)
    {
        if(!isHolding)
            isTouchingladder = isEnter;
    }

    protected override void ReleaseSkill()
    {
        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        if (cat != null && cat.isBeingHeld)
        {
            throwAimArrow.SetActive(false);
            cat.transform.rotation = Quaternion.identity;
            cat.transform.position = transform.position + (Vector3) (axisInput.normalized * throwOffset);
            //cat.GetComponent<Rigidbody2D>().velocity = (axisInput.normalized * throwStrength) + _rigidbody2D.velocity;
            cat.GetComponent<Rigidbody2D>().velocity = (axisInput.normalized * throwStrength);
            cat.isBeingHeld = false;
            cat.isBeingThrown = true;
            isHolding = false;
        }
    }

    protected override void UseSkill()
    {
        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        axisInput = axisInput.magnitude == 0 ? Vector2.up : axisInput;
        if (cat != null && (cat.transform.position - transform.position).magnitude < grabDistance)
        {
            if (axisInput.magnitude > 0)
            {
                throwAimArrow.SetActive(true);
                throwAimArrow.transform.localPosition = axisInput.normalized * throwAimArrowOffset;
                throwAimArrow.transform.up = axisInput.normalized * throwAimArrowOffset * 2;
            }
            else
            {
                throwAimArrow.SetActive(false);
            }
            isHolding = true;
            cat.isBeingHeld = true;
            //cat.transform.rotation = Quaternion.Euler(0,0,180);
            cat.transform.right = axisInput.normalized;
            cat.GetComponent<Rigidbody2D>().MovePosition(transform.position + (Vector3.up * flightSpan));
        }
    }
}
