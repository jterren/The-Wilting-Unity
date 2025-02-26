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

    public RuntimeAnimatorController Idle;
    public RuntimeAnimatorController AttackW;
    public RuntimeAnimatorController AttackI;

    // Use this for initialization
    void Start()
    {
        sprintSpeed = walkSpeed * (float)1.5;
        realSpeed = walkSpeed;
        animCurrent = 0;
        idleAnim = 0;
        attackAnim = 1;
        attackD = "";

        animator = this.GetComponent<Animator>();
        animator.SetInteger("isMove", 0);
        animator.SetInteger("DirY", 0);

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
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * realSpeed * Time.deltaTime, 0f, 0f));
            animator.SetInteger("DirY", -1);
            animator.SetInteger("isMove", 1);
            DirY = animator.GetInteger("DirY");
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * realSpeed * Time.deltaTime, 0f, 0f));
            animator.SetInteger("DirY", -1);
            animator.SetInteger("isMove", 1);
            DirY = animator.GetInteger("DirY");
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * realSpeed * Time.deltaTime, 0f));
            animator.SetInteger("DirY", 1);
            animator.SetInteger("isMove", 1);
            DirY = animator.GetInteger("DirY");
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * realSpeed * Time.deltaTime, 0f));
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
            }
            else if (animator.GetInteger("isMove") == 0)
            {
                animator.runtimeAnimatorController = AttackI as RuntimeAnimatorController;
                animator.SetInteger("DirY", DirY);
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