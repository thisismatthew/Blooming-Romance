using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CharacterState
{
    idle,
    walking,
    holding,
    pouring,
}

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private CharacterState currentState = CharacterState.idle;
    private CharacterState newState = CharacterState.idle;
    private bool facingRight = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement == Vector2.zero)
        {
            newState = CharacterState.idle;
        }
        else
        {
            if (movement.x > 0) facingRight = true;
            if (movement.x < 0) facingRight = false;
            newState = CharacterState.walking;
        }
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
        rb.MovePosition(rb.position + movement * moveSpeed);
    }

    private void UpdateAnimation()
    {
        if (newState != currentState)
        {
            currentState = newState;
            switch (currentState)
            {
                case CharacterState.idle:
                    if (facingRight) anim.Play("anim_OldLadyRight"); else anim.Play("anim_OldLadyLeft");
                    break;
                case CharacterState.walking:
                    if (facingRight) anim.Play("anim_OldWalkinRight"); else anim.Play("anim_OldWalkinLeft");
                    break;
            }
        }
    }
}
