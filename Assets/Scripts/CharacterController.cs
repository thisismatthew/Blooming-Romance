using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
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
    public CharacterState CurrentState = CharacterState.idle;
    private CharacterState newState = CharacterState.idle;
    private bool facingRight = false;
    public bool HoldingSeed;
    public PlantData CurrentSeedData;
    [SerializeField] private ParticleSystem waterDrops;
    private bool watering = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        DisableWaterInAnimation();
    }

    private void Update()
    {
        if (watering) return;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            watering = true;
            newState = CharacterState.pouring;
            Invoke("WateringDone", 3f);
        }
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
        if (CurrentState == CharacterState.walking)
        {
            rb.MovePosition(rb.position + movement * moveSpeed);
        }
    }

    private void UpdateAnimation()
    {
        if (newState != CurrentState)
        {
            CurrentState = newState;
            switch (CurrentState)
            {
                case CharacterState.idle:
                    if (facingRight) anim.Play("anim_OldLadyRight"); else anim.Play("anim_OldLadyLeft");
                    break;
                case CharacterState.walking:
                    if (facingRight) anim.Play("anim_OldWalkinRight"); else anim.Play("anim_OldWalkinLeft");
                    break;
                case CharacterState.pouring:
                    if (facingRight) anim.Play("anim_OldWateringRight"); else anim.Play("anim_OldWateringLeft");
                    break;
            }

        }
    }

    public void EnableWaterInAnimation()
    {
        waterDrops.enableEmission = true;
    }

    public void DisableWaterInAnimation()
    {
        waterDrops.enableEmission = false;
    }

    public void WateringDone()
    {
        watering = false;
    }
}
