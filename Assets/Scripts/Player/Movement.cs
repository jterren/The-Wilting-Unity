using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    public float walkSpeed;
    private float sprintSpeed;
    private float realSpeed;
    private Rigidbody2D body;
    private Vector2 movement;

    // Use this for initialization
    void Start()
    {
        sprintSpeed = walkSpeed * (float)1.5;
        realSpeed = walkSpeed;
        body = GetComponent<Rigidbody2D>();
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        realSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
    }

    void FixedUpdate()
    {
        movement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        body.MovePosition(body.position + realSpeed * Time.fixedDeltaTime * movement);
        animator.speed = realSpeed;

        GetMovement();
        SetSprites();
    }

    void GetMovement()
    {
        if (movement != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void SetSprites()
    {
        int direction = MathF.Sign(Input.GetAxis("Vertical"));
        if (!animator.GetBool("Vertical") && direction > 0)
        {
            animator.SetBool("Vertical", true);
        }
        else if (direction < 0)
        {
            animator.SetBool("Vertical", false);
        }
    }
}