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
    public float ledgeClimbTime = 1f;
    public PlayerController playerController = PlayerController.NONE;
    public bool isDead = false;
    public bool isSpritesFlipped = false;

    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    public FootCollider foot;
    public SideCollider leftSideCollider, rightSideCollider;
    public bool isTouchingladder = false;
    public bool isBeingPulled = false;
    public bool isBeingTethered = false;
    public bool isBeingHeld = false;
    public bool isBeingThrown = false;
    public bool isLedgeClimbing = false;
    public bool isHolding = false;
    public bool isLadderClimbing = false;

    private bool isLeftTouching;
    private bool isRightTouching;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isGamePaused = false;

    public void UpdateController(PlayerController p)
    {
        playerController = p;
    }

    public virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        foot.OnLandGround += () => { Land(); isGrounded = true; }; // Fix Bug for moving around floor tiles
        foot.OnLeaveGround += () => { isGrounded = false; };
        leftSideCollider.OnSideEnter += () => { isLeftTouching = true; }; // Fix issue for tiled walls
        leftSideCollider.OnSideExit += () => { isLeftTouching = false; };
        leftSideCollider.OnLedgeEnter += HandleLedge;
        rightSideCollider.OnSideEnter += () => { isRightTouching = true; };
        rightSideCollider.OnSideExit += () => { isRightTouching = false; };
        rightSideCollider.OnLedgeEnter += HandleLedge;

        //base.Start();
    }

    public virtual void Update()
    {        
        if (!(isLedgeClimbing || isBeingHeld || isDead || isGamePaused || playerController == PlayerController.NONE))
        {
            HandlePlayerSwitch();
            HandleAxisInput();
            HandleButtonInput();
        }

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
        GameManager.instance.GameOver();
    }

    public override void Pause()
    {
        isGamePaused = true;
    }

    public override void UnPause()
    {
        isGamePaused = false;
    }
    
    private void HandlePlayerSwitch()
    {
        bool pauseButtonPressed = Input.GetButtonDown("Pause");
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

    private void HandleButtonInput()
    {
        if (isLadderClimbing) return;

        bool jumpButtonPressed = Input.GetButtonDown(playerController.ToString() + "_Jump");
        bool skillButtonPressed = Input.GetButton(playerController.ToString() + "_Skill");
        bool skillButtonReleased = Input.GetButtonUp(playerController.ToString() + "_Skill");

        if (jumpButtonPressed && isGrounded)    
            Jump();
        if (skillButtonPressed)
            UseSkill();
        if (skillButtonReleased)
            ReleaseSkill();
    }

    private void HandleAxisInput()
    {
        if (isBeingTethered)
            return;

        if (isHolding)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            return;
        }

        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        if ((axisInput.x > 0 && isRightTouching) || (axisInput.x < 0 && isLeftTouching))
        {
            axisInput = new Vector2(0, axisInput.y);
        }

        if (isTouchingladder)
        {
            if (Mathf.Abs(axisInput.y) > 0)
            {
                StartLadderClimb();
            }
        }
        else
        {
            StopLadderClimb();
        }

        if (isLadderClimbing)
            HandleLadder(axisInput);
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

        if (isBeingThrown)
        {
            Vector2 sumVector = new Vector2((direction.x * moveSpeed) + _rigidbody2D.velocity.x, 0);
            if (Mathf.Abs(sumVector.x) < Mathf.Abs(_rigidbody2D.velocity.x) || Mathf.Abs(sumVector.x) < moveSpeed)
                _rigidbody2D.velocity += new Vector2((direction.x * moveSpeed), 0);
        }
        else if (isBeingPulled)
        {
            _rigidbody2D.velocity += new Vector2((direction.x * moveSpeed), 0);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.velocity.y);
        }
    }

    private void HandleLadder(Vector2 direction)
    {
        _rigidbody2D.velocity = new Vector2(direction.x, direction.y) * climbingSpeed;
    }

    private void StartLadderClimb()
    {
        isLadderClimbing = true;
        _animator.SetBool("isClimbing", true);
        _rigidbody2D.gravityScale = 0;
    }

    private void StopLadderClimb()
    {
        isLadderClimbing = false;
        _animator.SetBool("isClimbing", false);
        _rigidbody2D.gravityScale = 4;
    }

    private void HandleLedge(GameObject ledgeTile)
    {
        Vector2 axisInput = new Vector2(Input.GetAxisRaw(playerController.ToString() + "_Horizontal"), Input.GetAxisRaw(playerController.ToString() + "_Vertical"));
        if (isGrounded) return;
        if (ledgeTile.transform.position.x > transform.position.x && axisInput.x > 0)
            StartCoroutine(LedgeClimb(ledgeTile));
        if (ledgeTile.transform.position.x < transform.position.x && axisInput.x < 0)
            StartCoroutine(LedgeClimb(ledgeTile));
    }

    private IEnumerator LedgeClimb(GameObject target)
    {
        if (!isLedgeClimbing)
        {
            isLedgeClimbing = true;
            float timer = 0;
            Vector3 startingPosition = transform.position;
            _animator.SetBool("isClimbing", true);
            while (timer < ledgeClimbTime)
            {
                timer += Time.deltaTime;
                ////transform.Rotate(new Vector3(0, 0, 360 * (timer / ledgeClimbTime)));
                transform.position = Vector3.Lerp(target.transform.position, target.transform.position + Vector3.up, 0);
                yield return null;
            }
            _animator.SetBool("isClimbing", false);
            transform.position = target.transform.position + Vector3.up;
            isLedgeClimbing = false;
        }
    }

    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        _animator.SetBool("isJumping", true);
    }

    private void Land()
    {
        _animator.SetBool("isJumping", false);
        isBeingThrown = false;
        StopLadderClimb();
    }

    public abstract void LadderEnter(bool isEnter);

    protected abstract void UseSkill();

    protected abstract void ReleaseSkill();
}
