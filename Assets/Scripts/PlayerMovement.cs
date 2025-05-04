using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public Transform wallCheck;
    public Transform groundCheck;
    public LayerMask wallObjects;
    public LayerMask groundObjects;
    public bool unlockedDoubleJump = false;
    public bool unlockedDashing = false;
    public bool unlockedWallGrab = false;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDir, climbDir;

    // WallGrab variables
    public bool isWallSliding = false;
    public float wallSlideSpeed = 1.5f;
    public bool isWallJumping;
    public float wallJumpDir;
    public bool isFacingWall;
    public float wallJumpStrength;
    public bool canJumpFromWall;


    // Dash variables
    public bool pressedDash;
    public bool isDashing;
    public bool stoppedDashing;
    public bool canDash = true;
    public float dashDuration;
    public float dashCooldown;
    public float dashSpeed;
    
    
    // Jump variables
    public bool pressedJump;
    public bool isGrounded;
    private bool spentDoubleJump, isJumping;

    public Animator animator;
    public bool isWalking, isFalling, isGoingUp;

    public bool gotHit;
    public float knockbackForce, knockbackTime, knockbackTimeSince;
    public Vector3 knockbackDirection;


    public Tooltips tooltip;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spentDoubleJump = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        stoppedDashing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(knockbackTimeSince > 0)
        {
            knockbackTimeSince -= Time.deltaTime;
        } else {
            gotHit = false;
        }
        
        if (isDashing)
        {
            return;
        }

        if (gotHit)
        {
            return;
        }


        if (stoppedDashing && (isGrounded || isWallSliding))
        {
            canDash = true;
        }

        moveDir = Input.GetAxis("Horizontal");
        climbDir = Input.GetAxis("Vertical");


        isFacingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallObjects);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundObjects);
        if (isGrounded == true)
        {
            spentDoubleJump = false;
            isJumping = false;
        }

        if (isJumping && spentDoubleJump)
        {
            pressedJump = false;
        }

        if (Input.GetButtonDown("Jump") && (!isJumping || (unlockedDoubleJump && !spentDoubleJump)))
        {
            pressedJump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && stoppedDashing && moveDir != 0 && unlockedDashing && canDash)
        {
            pressedDash = true;
        }

        

        DeterminePlayerOrientation();


        DoAnimations();
    }

    private void DoAnimations()
    {
        if (moveDir != 0)
        { isWalking = true; }
        else
        { isWalking = false; }

        if (climbDir < 0)
        { isFalling = true; }
        else
        { isFalling = false; }

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);

        animator.SetBool("isDoubleJumping", spentDoubleJump);
        
        if(spentDoubleJump && isJumping)
        {
            animator.SetBool("isDoubleJumping", true);
        } else {
            animator.SetBool("isDoubleJumping", false);
        }

        if (rb.velocity.y > 0.1f)
        {
            isGoingUp = true;
            isFalling = false;
        } else if (rb.velocity.y < -0.1f)
        {
            isGoingUp = false;
            isFalling = true;
        } else
        {
            isGoingUp = false;
            isFalling = false;
        }

        animator.SetBool("GoingUp", isGoingUp);
        animator.SetBool("GoingDown", isFalling);
        animator.SetBool("isWallMounted", isWallSliding);
    }

    
    private void FixedUpdate()
    {

        if (gotHit)
        {
            rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackDirection.y * knockbackForce / 2);
            return;
        }

        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        if (isGrounded)
        {
            spentDoubleJump = false;
        }
        

        if(pressedDash && unlockedDashing && canDash && moveDir != 0)
        {
            StartCoroutine(PlayerDash()); 
        }

        WallSlide();
        WallJump();

        PlayerJump();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "DoubleJumpUpgrade")
        {
            unlockedDoubleJump = true;
            tooltip.EnableDoubleJumpTip();
        }
        if (collision.gameObject.tag == "DashUpgrade")
        {
            unlockedDashing = true;
            tooltip.EnableDashTip();
        }
        if (collision.gameObject.tag == "WallGrabUpgrade")
        {
            unlockedWallGrab = true;
            tooltip.EnableWallGrabTip();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gotHit = true;
            knockbackDirection = (-collision.transform.position + this.transform.position).normalized;
            knockbackTimeSince = knockbackTime;
        }
    }


    private void WallSlide()
    {
        if(isFacingWall && !isGrounded && moveDir != 0 && unlockedWallGrab)
        { 
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            spentDoubleJump = false;
            
        } else {
            isWallSliding = false;
        }
    }


    
    private void WallJump()
    {
        if(isWallSliding)
        {
            canJumpFromWall = true;
        }
        if(canJumpFromWall && pressedJump)
        {
            pressedJump = false;
            isWallJumping = true;
            FlipPlayer();
            moveDir *= -1;
            wallJumpDir = transform.localScale.x;
            rb.velocity = new Vector2(wallJumpDir * wallJumpStrength, wallJumpStrength * 2);
            canJumpFromWall = false;
        }
    }


    private IEnumerator PlayerDash()
    {
        isDashing = true;
        animator.SetBool("isDashing", isDashing);
        canDash = false;
        stoppedDashing = false;
        float realGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float movement = 0f;
        if (moveDir < 0) movement = -1f;
        if (moveDir > 0) movement = 1f;
        rb.velocity = new Vector2(movement * dashSpeed, 0);

        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = realGravity;
        isDashing = false;
        animator.SetBool("isDashing", isDashing);
        pressedDash = false;

        yield return new WaitForSeconds(dashCooldown);
        stoppedDashing = true;

    }


    private void PlayerJump()
    {
        if(isWallSliding)
        {
            return;
        }
        if (pressedJump == true && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            pressedJump = false;
            isJumping = true;
        }
        else if (pressedJump && !spentDoubleJump && unlockedDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            spentDoubleJump = true;
            pressedJump = false;
            isJumping = true;
        }
    }

    private void DeterminePlayerOrientation()
    {
        if (moveDir > 0 && !facingRight)
        {
            FlipPlayer();
        }
        else if (moveDir < 0 && facingRight)
        {
            FlipPlayer();
        }
    }

    private void FlipPlayer()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
