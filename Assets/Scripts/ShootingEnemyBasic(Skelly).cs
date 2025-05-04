using UnityEngine;

public class ShootingEnemyBasic : Enemy
{

    public int walkingSpeed;
    public float patrolRadius;
    public Transform player;
    public Vector3 directionToPlayer, unitCenterPosition;
    public float distanceToPlayer;
    public float moveDir;
    public bool goingRight;
    public float aggroRadius;

    public GameObject projectile;
    public Transform projectileSpawnPoint;
    private float timeToShoot = 2f;
    private float timeSinceShot;


    public GameObject lootDrop;
    public int maxCoinDropped = 3;

    public Animator animator;
    private bool isShooting;

    Rigidbody2D rb;

    public float timeToKnockback = 0.1f, timeSinceKnockback, knockbackForce = 40f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        moveDir = 1;
        goingRight = true;
        unitCenterPosition = transform.position;
        timeSinceShot = 0;
        distanceToPlayer = float.MaxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isShooting", isShooting);

        directionToPlayer = (player.position - transform.position).normalized;
        distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (timeSinceKnockback > 0)
        {
            timeSinceKnockback -= Time.deltaTime;
        }
        else
        {
            timeSinceKnockback = 0;
        }
    }

    private void FixedUpdate()
    {

        if(timeSinceKnockback <= 0)
        {
            if (Mathf.Abs(distanceToPlayer) <= aggroRadius)
            {
                isShooting = true;
                if (timeSinceShot <= 0)
                {
                    timeSinceShot = timeToShoot;
                    Attack();
                }
                else
                {
                    timeSinceShot -= Time.deltaTime;
                }
            }
            else
            {
                isShooting = false;
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
        if (goingRight && transform.position.x >= (unitCenterPosition.x + patrolRadius))
        {
            moveDir *= -1;
            goingRight = false;
            Flip();
        }
        else if (!goingRight && transform.position.x <= (unitCenterPosition.x - patrolRadius))
        {
            moveDir *= -1;
            goingRight = true;
            Flip();
        }
    }

    public override void Attack()
    {
        float rotZ = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(0f, 0f, rotZ - 45));
    }

    public override void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }

        if (timeSinceKnockback <= 0)
        {
            timeSinceKnockback = timeToKnockback;
        }
    }
    private void DropLoot()
    {
        int num = Random.Range(1, maxCoinDropped);
        for (int i = 0; i < num; ++i)
        {
            Instantiate(lootDrop, transform.position, Quaternion.identity);
        }
    }


    public override void Die()
    {
        DropLoot();
        Destroy(gameObject);
        this.enabled = false;
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(unitCenterPosition, patrolRadius);
        Gizmos.DrawWireSphere(unitCenterPosition, aggroRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            moveDir *= -1;
            goingRight = !goingRight;
            Flip();
        }
    }
}
