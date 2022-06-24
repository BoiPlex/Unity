using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float diveSpeed;
    [SerializeField] private LayerMask obstacleLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private float horizontalInput;
    private bool facingRight = true;
    private float normalGravity;

    private bool wallSliding = false;
    private float wallSlideSpeedMax = 1;
    private float wallJumpCooldown = 0;

    private bool hasDash = false;
    private bool isDashing = false;
    private float dashCooldown = 0;
    private float resetDashCooldown = 0.2f;

    private bool isDiving = false;

    private void Awake()
    {
        // Grab references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        normalGravity = body.gravityScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Turn left/right
        if (horizontalInput > 0.01f && !facingRight ||
            horizontalInput < -0.01f && facingRight)
            Flip();

        // Set animator parameters
        anim.SetBool("IsRunning", horizontalInput != 0);
        anim.SetBool("OnGround", isGrounded());
        anim.SetBool("OnWall", onWall());
        anim.SetBool("IsDashing", isDashing);
        anim.SetBool("IsDiving", isDiving);

        // Wall jump logic
        wallSliding = false;
        if (onWall() && !isGrounded() && body.velocity.y < 0 && !isDashing)
        {
            wallSliding = true;

            if (body.velocity.y < -wallSlideSpeedMax)
            {
                if (isDiving)
                    body.velocity = new Vector2(body.velocity.x, -wallSlideSpeedMax * diveSpeed * 3);
                else
                    body.velocity = new Vector2(body.velocity.x, -wallSlideSpeedMax);
            }
                
        }
        
        if (wallJumpCooldown > 0.2f)
        {
            // Move left/right
            body.velocity = new Vector2(horizontalInput * runSpeed, body.velocity.y);

            // Jump
            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;

        // Dash logic
        if (isGrounded() || onWall())
            hasDash = true;

        dashCooldown -= Time.deltaTime;
        
        if (dashCooldown < 0)
            dashCooldown = -1;
        else
            dashCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.O) && hasDash && dashCooldown <= 0)
            StartCoroutine(Dash());

        // Dive Logic
        if (Input.GetKeyUp(KeyCode.S) || isGrounded())
            isDiving = false;
        if (Input.GetKey(KeyCode.S) && !isGrounded() && body.velocity.y <= 0)
        {
            isDiving = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Abs(body.velocity.y) * -diveSpeed);
        }

    }

    private void Jump()
    {
        if (wallSliding)
        {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, jumpPower);
            
            if (Mathf.Abs(horizontalInput) < 0.1)
                Flip();

            wallJumpCooldown = 0;
        }

        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            //anim.SetTrigger("IsJumping");
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        hasDash = false;
        float startTime = Time.time;
        float localScaleX = transform.localScale.x;

        while (Time.time < startTime + dashTime)
        {
            float movementSpeed = dashSpeed * Time.deltaTime;

            Physics.gravity = new Vector2(0, 0);
            body.velocity = new Vector2(body.velocity.x, 0);

            if (onWall())
                Flip();

            if (facingRight)
                transform.Translate(movementSpeed, 0, 0);
            else
                transform.Translate(-movementSpeed, 0, 0);

            /*
            if (onWall())
                Jump();
            */

            dashCooldown = resetDashCooldown;
            yield return null;
        }
        isDashing = false;
        Physics.gravity = new Vector2(0, -normalGravity);
    }
    
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.4f, obstacleLayer);
        return raycastHit.collider != null;
    }
    
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.02f, obstacleLayer);
        return raycastHit.collider != null;
    }
    
    
}
