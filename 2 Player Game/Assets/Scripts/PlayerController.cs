using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float diveSpeed;
    
    public int maxExtraJumps;
    private int numExtraJumps = 0;

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode dive;
    public KeyCode shoot;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    public Transform groundCheckPoint;
    public float groundCheckRad;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask otherPlayerLayer;

    public bool isGrounded;
    public bool onIce;
    private float offGroundCooldown = 0;

    private Animator anim;

    public GameObject orb;
    public Transform shootPoint;

    private ManaBar manaBar;
    public float maxMana;
    private float mana;
    public float orbCost;
    public float manaPerSec;


    private float wallJumpCooldown = 0;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    private bool wallSliding = false;
    private RaycastHit2D wallCheckHit;
    private float jumpTime;
    
    private bool isDiving = false;
    private bool facingRight = true;

    public GameObject powerupCollectEffect;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        manaBar = GetComponent<ManaBar>();
        mana = maxMana;
}

    // Update is called once per frame
    void Update()
    {
        // Flipping
        if (body.velocity.x > 0.1f)
            FaceRight(true);
        else if (body.velocity.x < -0.1f)
            FaceRight(false);

        if (wallJumpCooldown > 0.2f)
        {
            // Move left/right
            // Ice physics
            if (onIce && (isGrounded || offGroundCooldown < 0.5f || wallJumpCooldown > 1))
            {
                if (Input.GetKey(left))
                {
                    body.velocity += new Vector2(-moveSpeed * Time.deltaTime * 3, 0);
                }
                else if (Input.GetKey(right))
                {
                    body.velocity += new Vector2(moveSpeed * Time.deltaTime * 3, 0);
                }
                else
                {
                    offGroundCooldown += Time.deltaTime;
                    if (offGroundCooldown > 0.5f)
                        offGroundCooldown = 0;
                }
            }
            // Normal physics
            else if (!onIce)
            {
                if (Input.GetKey(left))
                {
                    body.velocity = new Vector2(-moveSpeed, body.velocity.y);
                }
                else if (Input.GetKey(right))
                {
                    body.velocity = new Vector2(moveSpeed, body.velocity.y);
                }
                else
                {
                    body.velocity = new Vector2(0, body.velocity.y);
                }
            }

            // Jump
            if (Input.GetKeyDown(jump))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;

        bool touchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRad, groundLayer) ||
                              Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRad, platformLayer);
        bool touchingOtherPlayer = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRad, otherPlayerLayer);
        if (touchingGround || touchingOtherPlayer)
        {
            isGrounded = true;
            jumpTime = Time.time + wallJumpTime;

            numExtraJumps = maxExtraJumps;
        }
        else if (jumpTime < Time.time)
        {
            isGrounded = false;
        }

        // Wall jump logic
        if (facingRight)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
            //Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.red);
        }
        else if (!facingRight)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            //Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.red);
        }
        
        if (wallCheckHit && !isGrounded && body.velocity.x != 0)
        {
            wallSliding = true;
            jumpTime = Time.time + wallJumpTime;

            numExtraJumps = maxExtraJumps;
        }
        else if (jumpTime < Time.time)
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            if (onIce)
            {
                if (isDiving)
                    body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, wallSlideSpeed * diveSpeed * 4.5f, float.MaxValue));
                else
                    body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, wallSlideSpeed * 2.5f, float.MaxValue));
            }
            else
            {
                if (isDiving)
                    body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, wallSlideSpeed * diveSpeed * 4.5f, float.MaxValue));
                else
                    body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, wallSlideSpeed, float.MaxValue));
            }
        }

        // Dive Logic
        if (Input.GetKeyUp(dive) || isGrounded)
            isDiving = false;
        if (Input.GetKey(dive)) // && !isGrounded
        {
            isDiving = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Abs(body.velocity.y) * -diveSpeed);
        }

        // Shoot (5 mana per orb)
        if (Input.GetKeyDown(shoot) && mana >= 5)
        {
            GameObject orbClone = (GameObject)Instantiate(orb, shootPoint.position, shootPoint.rotation);
            orbClone.transform.localScale = transform.localScale;
            anim.SetTrigger("Shoot");
            mana -= 5;
        }
        if (mana > 100)
            mana = 100;
        else
            mana += Time.deltaTime * manaPerSec;

        // Update mana bar
        manaBar.SetHealth(mana);

        // Animation parameters
        anim.SetFloat("X_Speed", Mathf.Abs(body.velocity.x));
        anim.SetFloat("Y_Speed", body.velocity.y);
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("IsDiving", isDiving);
    }

    public void Hit()
    {
        anim.SetTrigger("Hit");
    }

    public void Die()
    {
        body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        anim.SetTrigger("Die");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.CompareTag("OrbP1") || other.gameObject.CompareTag("OrbP2")))
        {
            if (other.gameObject.CompareTag("PowerupHealth"))
            {
                if (this.gameObject.CompareTag("Player1"))
                    FindObjectOfType<GameManager>().HealP1();
                if (this.gameObject.CompareTag("Player2"))
                    FindObjectOfType<GameManager>().HealP2();
            }
            else if (other.gameObject.CompareTag("PowerupJump"))
            {
                jumpForce *= 1.1f;
                //body.gravityScale -= ;
            }
            else if (other.gameObject.CompareTag("PowerupMana"))
            {
                mana += maxMana * 0.25f;
            }
            else if (other.gameObject.CompareTag("PowerupSpeed"))
            {
                moveSpeed *= 1.1f;
            }

            Instantiate(powerupCollectEffect, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            FindObjectOfType<SpawnPowerup>().PowerupCollected();
        }
    }

    void Jump()
    {
        if (wallSliding)
        {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * moveSpeed * 0.75f, jumpForce);

            if (facingRight)
                FaceRight(false);
            else
                FaceRight(true);

            wallJumpCooldown = 0;
        }

        else if (isGrounded) // Grounded jump
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        else if (!isGrounded && numExtraJumps > 0) // Air jump
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            numExtraJumps--;
            anim.SetTrigger("ExtraJumping");
        }

    }

    void FaceRight(bool isRight)
    {
        if (isRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (!isRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
