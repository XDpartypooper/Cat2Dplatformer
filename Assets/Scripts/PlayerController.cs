using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Basemovespeed = 5f;
    [SerializeField] private float CurrentMovespeed;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private float jumppower = 5f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning;
    [SerializeField] private LayerMask GroundLayer;
    public float GroundCheckDistance;

    [Header("Cat meow")]
    [SerializeField] GameObject MeowPrefab;


    float horizontalInput;
    private Rigidbody2D rb;
    private Animator animator;
    private bool MCD;//meow cool down
    private bool CanMove = true;

    void Awake()
    {
        CurrentMovespeed = Basemovespeed;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        HandleCollision();
        MovementAnimation();
        if (CanMove)
        {
            rb.linearVelocity = new Vector2(horizontalInput * CurrentMovespeed, rb.linearVelocity.y);
        }     
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 Move = context.ReadValue<Vector2>();
        horizontalInput = Move.x;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (rb.linearVelocity.y < -0.1f) return;
        if (!CanMove) return;

        if (context.started && isGrounded)
        {
            animator.SetBool("IsJumping", true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumppower);

        }
    }

    public void OnMeow(InputAction.CallbackContext context)
    {
        if (MCD)
        {
            return;
        }

        if (context.started && isGrounded)
        {
            animator.SetTrigger("Meow");
            Vector3 meowPos = transform.position + (transform.right * 1.2f * Mathf.Sign(transform.localScale.x));
            GameObject meow = Instantiate(MeowPrefab, meowPos, transform.rotation);
            meow.transform.localScale = transform.localScale;

            meow.GetComponent<AudioSource>().Play();
            Destroy(meow, 1);
            MCD = true;
            CanMove = false;
            StartCoroutine(MeowCoolDown());
        }
    }

    public void OnLoaf(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            animator.SetTrigger("LOAF");
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {


        if (context.started)
        {
            if (isGrounded == false) return;
            CurrentMovespeed = Basemovespeed * 1.5f;
        }
        else if (context.canceled)
        {
            CurrentMovespeed = Basemovespeed;
        }
    }
    void MovementAnimation()
    {
        if (!isGrounded)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }



        animator.SetFloat("XVel", rb.linearVelocity.x);
        animator.SetFloat("YVel", rb.linearVelocity.y);


        FlipSprite();
    }

    private void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -GroundCheckDistance));
    }

    private void HandleCollision()
    {
        //might not need this anymore
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, GroundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }

    IEnumerator MeowCoolDown()
    {
        rb.linearVelocity = new Vector2(0,0);
        yield return new WaitForSeconds(1.0f);
        CanMove = true;
        yield return new WaitForSeconds(0.5f);
        MCD = false;
    }
}
