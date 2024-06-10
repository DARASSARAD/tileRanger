using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private float moveHorizontal, moveVertical;
    [Header("Movement")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField] private float fallGravity;
    public bool isGrounded = false;
    public bool facingRight = true;

    [Header("Effects and Audio")]
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] landSounds;
    public GameObject dashEffect;

    [Header("Dash Settings")]
    public float dashingVelocity;
    public float dashingTime;
    public float dashingCooldown;
    private bool isDashing;
    private bool canDash = true;
    private bool doubleJump;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private TrailRenderer trial;
    bool clickLeft, clickRight, clickJump, clickDash;
    bool isAlive = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trial = GetComponent<TrailRenderer>();
    }

    void Update()
    {

        if (!isAlive)
        {
            return;
        }
        if (isDashing)
        {
            return;
        }


        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);


        if (moveHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }


        Jump();



        DashButton();



    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButton(0))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); //jump
                jumpParticles.Play();
                doubleJump = true;
                isGrounded = false;
            }

            else if (doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                doubleJump = false;
            }

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) //varible jump height 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }

        //increase gravity when reach peak jump height
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = gravityScale;
        }
        else
        {
            rb.gravityScale = fallGravity;
        }

    }
    public void DashButton()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

    }
    private void FixedUpdate()
    {
        MoveButton();

    }

    private void MoveButton()
    {
        if (isDashing)
        {
            return;
        }
        //horizontal movement
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        trial.emitting = true;
    }

    public void JumpButton(bool addddd)
    {
        clickJump = addddd;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            audioSource.PlayOneShot(landSounds[Random.Range(0, landSounds.Length)]);
        }
    }
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale; //storing the original gravity
        rb.gravityScale = 0f;

        //main dashing line
        rb.velocity = new Vector2(transform.localScale.x * dashingVelocity, 0f);
        //Instantiate(dashEffect, transform.position, Quaternion.identity); 

        yield return new WaitForSeconds(dashingTime);

        //after dash

        rb.gravityScale = originalGravity;
        isDashing = false;

        //dash cooldown
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }




}
