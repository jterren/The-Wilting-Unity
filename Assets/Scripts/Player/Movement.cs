using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Animator animator;
    public float walkSpeed;
    private float sprintSpeed;
    private float realSpeed;
    public int animCurrent;
    private int idleAnim;
    private int attackAnim;
    public string attackD;
    private int DirY;
    private int DirX;
    public RuntimeAnimatorController Idle;
    public RuntimeAnimatorController AttackW;
    public RuntimeAnimatorController AttackI;
    private Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        sprintSpeed = walkSpeed * (float)1.5;
        realSpeed = walkSpeed;
        animCurrent = 0;
        idleAnim = 0;
        attackAnim = 1;
        attackD = "";
        body = GetComponent<Rigidbody2D>();

        animator = this.GetComponent<Animator>();
        animator.SetInteger("isMove", 0);
        animator.SetInteger("DirY", 0);
        animator.SetInteger("isMove", 0);
        animator.SetInteger("DirX", 0);
        animator.speed = walkSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        DirY = animator.GetInteger("DirY");
        if (Input.anyKeyDown == false)
        {
            animator.SetInteger("isMove", 0);
            animator.SetInteger("AttackL", 0);
            animator.SetInteger("AttackR", 0);
        }

        realSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        animCurrent = (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) ? attackAnim : idleAnim;

        FindMouseClick();
        GetMovement();
        SetAnim();
    }

    void FixedUpdate()
    {
        Vector2 movement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        body.MovePosition(body.position + realSpeed * Time.fixedDeltaTime * movement);
        animator.speed = realSpeed;
    }


    void FindMouseClick()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetInteger("AttackL", animCurrent);
            attackD = "Left";
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            animator.SetInteger("AttackR", animCurrent);
            attackD = "Right";
        }
        else
        {
            attackD = "";
        }

    }

    void GetMovement()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            animator.SetInteger("DirX", 1);
            animator.SetInteger("isMove", 1);
            DirX = animator.GetInteger("DirX");
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            animator.SetInteger("DirX", -1);
            animator.SetInteger("isMove", 1);
            DirX = animator.GetInteger("DirX");
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            animator.SetInteger("DirY", 1);
            animator.SetInteger("isMove", 1);
            DirY = animator.GetInteger("DirY");
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            animator.SetInteger("DirY", -1);
            animator.SetInteger("isMove", 1);
            DirY = animator.GetInteger("DirY");
        }
    }

    void SetAnim()
    {
        if (animCurrent == attackAnim)
        {
            if (animator.GetInteger("isMove") == 1)
            {
                animator.runtimeAnimatorController = AttackW as RuntimeAnimatorController;
                animator.SetInteger("DirY", DirY);
                animator.SetInteger("DirX", DirX);
            }
            else if (animator.GetInteger("isMove") == 0)
            {
                animator.runtimeAnimatorController = AttackI as RuntimeAnimatorController;
                animator.SetInteger("DirY", DirY);
                animator.SetInteger("DirX", DirX);
            }
        }
        else if (animCurrent == idleAnim)
        {
            animCurrent = idleAnim;
            animator.runtimeAnimatorController = Idle as RuntimeAnimatorController;
            animator.SetInteger("DirY", DirY);
        }
    }
}