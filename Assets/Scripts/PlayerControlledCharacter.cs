using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledCharacter : MonoBehaviour, IKillable, IHurtable
{

    public enum PlayerController { Player1, Player2, NONE }

    public int health = 5;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float airControl = 2f;
    public PlayerController playerController = PlayerController.NONE;
    public bool isDead = false;
    public bool isSpritesFlipped = false;
    public bool tetherIsPulling = false;

    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
     
    protected bool isFacingRight = true;
    protected bool isGrounded;

    public void UpdateController(PlayerController p)
    {
        playerController = p;
    }

    public virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    { 
        HandleCharacterSwappingInput(); 
    }
  
    protected virtual void HandleCharacterSwappingInput()
    {
        if (Input.GetButtonDown("Pause"))
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

    protected virtual void Move(Vector2 direction)

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

        if (Mathf.Abs(direction.x) > 0.1) _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.velocity.y);

        //if (isGrounded)
        //    if (Mathf.Abs(direction.x) >= 0.1)
        //        _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.velocity.y);
        //else
        //    _rigidbody2D.velocity += new Vector2(direction.x * moveSpeed, 0) * Time.deltaTime * airControl;


    }

    protected virtual void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        _animator.SetBool("isJumping", true);
    }

    protected virtual void Land()
    {
        _animator.SetBool("isJumping", false);
    }

    public virtual void GetHurt(int damage) {
        health = (int) Mathf.Clamp(health - damage, 0, Mathf.Infinity);
        _animator.SetTrigger("getHit");

        if (health <= 0)
            Die();
    }

    public virtual void Die() {
        isDead = true;
        _animator.SetBool("isDead", true);
        _rigidbody2D.velocity = Vector2.zero;
    }
}
