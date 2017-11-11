using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : PlayerControlledCharacter
{
    public float throwStrength = 100f;
    public float grabDistance = 2f;

    private Cat cat;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //cat = collision.GetComponent<Cat>();
    }

    public override void Start()
    {
        base.Start();
        cat = GameObject.Find("Cat").GetComponent<Cat>();
    }

    public override void Update()
    {
        if (isBeingPulled)
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
        isTouchingladder = isEnter;
    }

    protected override void ReleaseSkill()
    {
        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        if (cat != null && cat.isBeingHeld)
        {
            cat.transform.rotation = Quaternion.identity;
            cat.GetComponent<Rigidbody2D>().velocity = (axisInput.normalized * throwStrength) + _rigidbody2D.velocity;
            cat.isBeingHeld = false;
            cat.isBeingThrown = true;
        }
    }

    protected override void UseSkill()
    {
        if (cat != null && !cat.isBeingThrown && (cat.transform.position - transform.position).magnitude < grabDistance)
        {
            cat.isBeingHeld = true;
            cat.transform.rotation = Quaternion.Euler(0,0,180);
            cat.GetComponent<Rigidbody2D>().MovePosition(transform.position + Vector3.up);
        }
    }
}
