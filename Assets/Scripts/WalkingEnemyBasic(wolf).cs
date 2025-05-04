using UnityEngine;

public class WalkingEnemyBasic : Enemy
{

    public int walkingSpeed;
    public float patrolRadius;
    public Transform player;
    public Vector3 directionToPlayer, unitCenterPosition;
    public float distanceToPlayer;
    public float moveDir;
    public bool goingRight;

    public GameObject lootDrop;
    public int maxCoinDropped = 3;

    Rigidbody2D rb;

    public Animator animator;
    public bool isAttacking = false;

    public float timeToKnockback = 0.15f, timeSinceKnockback, knockbackForce = 40f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        moveDir = 1;
        goingRight = true;
        unitCenterPosition = transform.position;
    }
    void Start()
    {
        maxHealth = 2;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isAttacking", isAttacking); ;
        directionToPlayer = (player.position - transform.position).normalized;
        distanceToPlayer = Vector2.Distance(player.position, unitCenterPosition);

        if(timeSinceKnockback > 0)
        {
            timeSinceKnockback -= Time.deltaTime;
        } else
        {
            timeSinceKnockback = 0;
        }

    }

    private void FixedUpdate()
    {
        if(timeSinceKnockback <= 0)
        {
            if(distanceToPlayer <= patrolRadius)
            {
                isAttacking = true;
                WalkToPlayer();
            } else {
                isAttacking = false;
                Patrol();
            }
        } else {
            float dirToKnock = directionToPlayer.x < 0 ? -1 : 1;
            rb.velocity = new Vector2(-dirToKnock * knockbackForce, rb.velocity.y);
        }

    }

    public void Patrol()
    {
        rb.velocity = new Vector2(moveDir * walkingSpeed, rb.velocity.y);
        if(goingRight && transform.position.x > (unitCenterPosition.x + patrolRadius))
        {
            moveDir *= -1;
            goingRight = false;
            Flip();
        } else if (!goingRight && transform.position.x < (unitCenterPosition.x - patrolRadius)) {
            moveDir *= -1;
            goingRight = true;
            Flip();
        }
    }

    public void WalkToPlayer()
    {
        rb.velocity = new Vector2(directionToPlayer.x * walkingSpeed * 1.5f, rb.velocity.y);
        if(directionToPlayer.x < 0)
        {
            if (goingRight == true)
            {
                Flip();
            }
            goingRight = false;
            moveDir = -1;
        } else
        {
            if (goingRight == false)
            {
                Flip();
            }
            goingRight = true;
            moveDir = 1;
        }
    }

    public override void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
        if(timeSinceKnockback <= 0)
        {
            timeSinceKnockback = timeToKnockback;
        }
    }


    public override void Attack()
    {

    }

    private void DropLoot()
    {
        int num = Random.Range(1, maxCoinDropped);
        for(int i = 0; i < num; ++i)
        {
            Instantiate(lootDrop, transform.position, Quaternion.identity);
        }
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    public override void Die()
    {
        DropLoot();
        Destroy(gameObject);
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(unitCenterPosition, patrolRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            moveDir *= -1;
            goingRight = !goingRight;
            Flip();
        }
    }

}
