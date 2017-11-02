using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : PlayerControlledCharacter {

    public FootCollider foot;
    public SideCollider leftSideCollider, rightSideCollider;
    public bool isGrounded;
    public bool isLeftTouching;
    public bool isRightTouching;

    public override void Start()
    {
        UpdateController(PlayerController.Player1);
        foot.OnLandGround += () => { Land(); isGrounded = true; }; // Fix Bug for moving around floor tiles
        foot.OnLeaveGround += () => {  };
        leftSideCollider.OnSideEnter += () => { isLeftTouching = true; }; // Fix issue for tiled walls
        leftSideCollider.OnSideExit += () => { isLeftTouching = false; };
        rightSideCollider.OnSideEnter += () => { isRightTouching = true; };
        rightSideCollider.OnSideExit += () => { isRightTouching = false; };

        base.Start();
    }

    public void Update()
    {
        // Dead
        if (isDead)
            return;

        // Handle Updates

        // If no player controlling, return
        if (playerController == PlayerController.NONE)
            return;

        // Handle Input
        HandleAxisInput();
        HandleButtonInput();
    }

    private void HandleAxisInput()
    {
        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        if ((axisInput.x > 0 && isRightTouching) || (axisInput.x < 0 && isLeftTouching))
        {
            axisInput = Vector2.zero;
        }

        Move(axisInput);
    }

    private void HandleButtonInput()
    {
        bool jumpButtonPressed = Input.GetButtonDown(playerController.ToString() + "_Jump");
        bool skillButtonPressed = Input.GetButtonDown(playerController.ToString() + "_Skill");
        if (jumpButtonPressed && isGrounded)
        {
            isGrounded = false;
            Jump();
        }

        if (skillButtonPressed)
        {
            GetHurt(1);
        }
    }
}
