using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatRunnerScript : MonoBehaviour
{
    [Header("Cat Movements")]
    [SerializeField] private bool CanMove = true;
    [SerializeField] private bool isSprinting = false;
    
    [SerializeField] private float Basemovespeed = 5f;
    [SerializeField] private float CurrentMovespeed;
   
    [Header("JUMP CHECK")]
    [SerializeField] private float jumppower = 5f;
    [SerializeField] private float jumptimer;
    [SerializeField] private float jumptime = 0.3f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool HoldingJump = false;
    [Header("Cat other info")]
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Transform FeetPos;
    [SerializeField] private float GroundDis = 0.25f;
    private float horizontalInput = 1f;// 1f move right , -1f move left
    private Rigidbody2D rb;
    private Animator animator;

    
    void Start()
    {
        CurrentMovespeed = Basemovespeed;//current speed to base speed
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGrounded();//ground check 
        IfHoldingJump();// check if player is jumping + holding button
        SprintCheck();//sprint check
        MovementAnimation();//update cat animations

    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(FeetPos.position, GroundDis, GroundLayer);
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        //if (!CanMove) return;
        if (context.started && isGrounded == true) //if pressed
        {         
            isJumping = true;
            HoldingJump = true;
            rb.linearVelocity = Vector2.up * jumppower;       
        }

        if (context.canceled) //if let go
        {
            HoldingJump = false;
            isJumping = false;
            jumptimer = 0;
        }

    }

    private void IfHoldingJump()
    {
        if (isGrounded) return; //Grounded
        if (!isJumping) return;// not jumping

        if (HoldingJump)
        {
            if (jumptimer < jumptime)
            {
                rb.linearVelocity = Vector2.up * jumppower;
                jumptimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        
    }


    public void SprintCheck()
    {
        if (isSprinting)
        {

            CurrentMovespeed = Basemovespeed * 1.5f;
        }
        else
        {
            CurrentMovespeed = Basemovespeed;
        }
    }
    void MovementAnimation()
    {
        if (isJumping) { animator.SetBool("IsJumping", true); }else{ animator.SetBool("IsJumping", false); };//set animator settings
        if (!isJumping && !isGrounded) { animator.SetBool("IsFalling", true); } else { animator.SetBool("IsFalling", false); };//set animator settings
        if (isGrounded) { animator.SetBool("IsGrounded", true); } else { animator.SetBool("IsGrounded", false); };//set animator settings

        animator.SetFloat("XVel", CurrentMovespeed);
        animator.SetFloat("YVel", rb.linearVelocity.y);
    }
}

