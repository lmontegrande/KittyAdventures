using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlledCharacter : Character
{

    public enum PlayerController { Player1, Player2, NONE }

    public int health = 5;
    public float moveSpeed = 5f;
    public float climbingSpeed = 5f;
    public float jumpForce = 5f;
    public float airControl = 2f;
    public PlayerController playerController = PlayerController.NONE;
    public bool isDead = false;
    public bool isSpritesFlipped = false;

    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    public FootCollider foot;
    public SideCollider leftSideCollider, rightSideCollider;
    public bool isTouchingladder = false;

    private bool isLeftTouching;
    private bool isRightTouching;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isGamePaused = false;
    public bool isClimbing = false;

    public void UpdateController(PlayerController p)
    {
        playerController = p;
    }

    public override void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        foot.OnLandGround += () => { Land(); isGrounded = true; }; // Fix Bug for moving around floor tiles
        foot.OnLeaveGround += () => { isGrounded = false; };
        leftSideCollider.OnSideEnter += () => { isLeftTouching = true; }; // Fix issue for tiled walls
        leftSideCollider.OnSideExit += () => { isLeftTouching = false; };
        rightSideCollider.OnSideEnter += () => { isRightTouching = true; };
        rightSideCollider.OnSideExit += () => { isRightTouching = false; };

        base.Start();
    }

    public void Update()
    {
        // Dead
        if (isDead || isGamePaused || playerController == PlayerController.NONE)
            return;

        HandleAxisInput();
        HandleButtonInput();
    }

    public void GetHurt(int damage)
    {
        health = (int)Mathf.Clamp(health - damage, 0, Mathf.Infinity);
        _animator.SetTrigger("getHit");

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        isDead = true;
        _animator.SetBool("isDead", true);
        _rigidbody2D.velocity = Vector2.zero;
    }

    public override void Pause()
    {
        isGamePaused = true;
    }

    public override void UnPause()
    {
        isGamePaused = false;
    }
    
    private void HandleButtonInput()
    {
        bool jumpButtonPressed = Input.GetButtonDown(playerController.ToString() + "_Jump");
        bool skillButtonPressed = Input.GetButtonDown(playerController.ToString() + "_Skill");
        bool pauseButtonPressed = Input.GetButtonDown("Pause");
        if (jumpButtonPressed && isGrounded)
        {             
            Jump();
        }

        if (skillButtonPressed)
        {
            UseSkill();
        }
        if (pauseButtonPressed)
        {
            switch (playerController)
            {
                case PlayerController.Player1:
                    UpdateController(PlayerController.Player2);
                    break;
                case PlayerController.Player2:
                    UpdateController(PlayerController.Player1);
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleAxisInput()
    {
        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        if ((axisInput.x > 0 && isRightTouching) || (axisInput.x < 0 && isLeftTouching))
        {
            axisInput = Vector2.zero;
        }

        if (isTouchingladder)
        {
            if (axisInput.y > 0)
            {
                isClimbing = true;
                _rigidbody2D.gravityScale = 0;
            }
        }
        else
        {
            isClimbing = false;
            _rigidbody2D.gravityScale = 4;
        }

        if (isClimbing)
            HandleClimbing(axisInput);
        else
            Move(axisInput);
    }

    private void Move(Vector2 direction)
    {
        _animator.SetFloat("movement", _rigidbody2D.velocity.magnitude);

        if (direction.x > 0)
        { 
            if (direction.x <= -0.1)
            {
                _spriteRenderer.flipX = isSpritesFlipped ? false : true;
                isFacingRight = false;
            }
            if (direction.x >= 0.1)
            {
                _spriteRenderer.flipX = isSpritesFlipped ? true : false;
                isFacingRight = true;
            }
        } else { 
            if (_rigidbody2D.velocity.x <= -0.1)
            {
                _spriteRenderer.flipX = isSpritesFlipped ? false : true;
                isFacingRight = false;
            }
            if (_rigidbody2D.velocity.x >= 0.1)
            {
                _spriteRenderer.flipX = isSpritesFlipped ? true : false;
                isFacingRight = true;
            }
        }

        if (isGrounded)
            _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.velocity.y);
        else
        {
            _rigidbody2D.velocity += new Vector2(direction.x * moveSpeed, 0);
            _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -moveSpeed, moveSpeed), _rigidbody2D.velocity.y);
        }
    }

    private void HandleClimbing(Vector2 direction)
    {
        _rigidbody2D.velocity = direction * climbingSpeed;
    }

    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        _animator.SetBool("isJumping", true);
    }

    private void Land()
    {
        _animator.SetBool("isJumping", false);
        isClimbing = false;
    }

    public abstract void LadderEnter(bool isEnter);

    protected abstract void UseSkill();
}
