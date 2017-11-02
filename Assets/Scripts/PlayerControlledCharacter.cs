﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledCharacter : MonoBehaviour, IKillable, IHurtable
{

    public enum PlayerController { Player1, Player2, NONE }

    public int health = 5;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public PlayerController playerController = PlayerController.NONE;

    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected bool isDead = false;
    protected bool isFacingRight = true;

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

    protected virtual void Move(Vector2 direction)

    {
        _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.velocity.y);
        _animator.SetFloat("movement", Mathf.Abs(direction.x));
        if (direction.x <= -0.1) {
            _spriteRenderer.flipX = true;
            isFacingRight = false;
        }
        if (direction.x >= 0.1) {
            _spriteRenderer.flipX = false;
            isFacingRight = true;
        }
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
