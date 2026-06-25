using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("Cat Movements")]
    [SerializeField] private float Basemovespeed = 5f;
    [SerializeField] private float CurrentMovespeed;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private float jumppower = 5f;
    //[SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning;
    [SerializeField] private LayerMask GroundLayer;
    public float GroundCheckDistance;
    public NPC NPCAT;
    public GameObject InteractiveGameobject;

    [Header("Cat meow")]
    [SerializeField] GameObject MeowPrefab;
    [SerializeField] GameObject MeowAttackPrefab;
    [SerializeField] bool MeowAttackUnlock;
    [SerializeField] GameObject AttackUI;

    [Header("Cat other info")]
    float horizontalInput;
    private Rigidbody2D rb;
    private Animator animator;
    public bool MCD;//meow cool down
    public bool CanMove = true;
    public bool Talking = false;
    [SerializeField] GameObject EndGameUI;

    void Awake()
    {
        CurrentMovespeed = Basemovespeed;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        MeowAttackUnlock = false;
        AttackUI.SetActive(false);
        EndGameUI.SetActive(false);
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
        if (!CanMove) return;

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
        if (!CanMove) return;

        if (context.started && isGrounded)
        {
            animator.SetTrigger("Meow");
            Vector3 meowPos = transform.position + (transform.right * 1.2f * Mathf.Sign(transform.localScale.x));
            GameObject meow = Instantiate(MeowPrefab, meowPos, transform.rotation);
            meow.transform.localScale = transform.localScale;

            meow.GetComponent<AudioSource>().volume = 0.4f;
            meow.GetComponent<AudioSource>().Play();
            Destroy(meow, 1);
            MCD = true;//meow cool down
            CanMove = false;

            if (NPCAT != null) NPCAT.StartDialouge();

            StartCoroutine(MeowCoolDown());
        }
    }


    public void OnATTACKMeow(InputAction.CallbackContext context)
    {
        if (MCD || MeowAttackUnlock == false)
        {
            return;
        }
        if (!CanMove) return;

        if (context.started && isGrounded)
        {
            animator.SetTrigger("Meow");
            Vector3 meowPos = transform.position + (transform.right * 1.2f * Mathf.Sign(transform.localScale.x));
            GameObject meow = Instantiate(MeowPrefab, meowPos, transform.rotation);
            meow.transform.localScale = transform.localScale;
            meow.GetComponent<AudioSource>().volume = 1;
            meow.GetComponent<AudioSource>().Play();


            Vector3 meowAttackPos = transform.position + (transform.right * 3.0f * Mathf.Sign(transform.localScale.x) + (Vector3.up * 0.9f));
            GameObject meowAttack = Instantiate(MeowAttackPrefab, meowAttackPos, transform.rotation);
            meowAttack.transform.localScale = transform.localScale;

            Destroy(meow, 1);
            Destroy(meowAttack, 1.4f);
            MCD = true;//meow cool down
            CanMove = false;

            if (InteractiveGameobject != null)
            {
                Debug.Log("Sending message: StartAction");
                InteractiveGameobject.SendMessage("StartAction", SendMessageOptions.DontRequireReceiver);

            }


            StartCoroutine(MeowCoolDown());
        }
    }

    public void unlockSkill()
    {
        MeowAttackUnlock = true;
        AttackUI.SetActive(true);
    }

  
    public void SetCATNPC(NPC CAT)// cat npc
    {
        NPCAT = CAT.GetComponent<NPC>();
    }
    public void SetCATNPC(GameObject Obj)//interactive
    {
        InteractiveGameobject = Obj;
    }
    public void RemoveCATNPC()//remove cat or interative
    {
        NPCAT = null;
        InteractiveGameobject = null;
    }

    public void OnLoaf(InputAction.CallbackContext context)
    {
        if (!CanMove) return;

        if (context.started)
        {
            animator.SetTrigger("LOAF");
            if (NPCAT != null) NPCAT.FollowLoaf();
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


        if (!CanMove) return;
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


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    IEnumerator MeowCoolDown()
    {
        rb.linearVelocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1.0f);
        CanMove = true;
        yield return new WaitForSeconds(0.5f);
        MCD = false;
    }
    public void TakeDamage()
    {
        StartCoroutine(TakingDamage());
    }
    IEnumerator TakingDamage()
    {
        CanMove = false;
        yield return new WaitForSeconds(0.5f);
        CanMove = true;
    }

    public void EndofGame()// short thing 
    {
        CanMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        StartCoroutine(CatEndAnimation());
    }

    IEnumerator CatEndAnimation()
    {
        CanMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        yield return new WaitForSeconds(0.5f);
        animator.Play("CatLick1");
        yield return new WaitForSeconds(1.5f);     
        animator.Play("CatStretching");
        yield return new WaitForSeconds(1.5f);
        animator.Play("CatLick2");
        yield return new WaitForSeconds(1.5f);
        

        EndGameUI.SetActive(true);
    }
}
