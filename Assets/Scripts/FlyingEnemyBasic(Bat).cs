using UnityEngine;

public class FlyingEnemyBasic : Enemy
{

    public int flyingSpeed;
    public float minXPatrol, maxXPatrol;
    public float minYPatrol, maxYPatrol;
    public float patrolRadiusX, patrolRadiusY;
    public Vector3 unitCenterPosition;
    public float moveDirX = 1, moveDirY = 0;
    public bool goingRight, goingUp;
    public bool canBob = true;

    public GameObject lootDrop;
    public int maxCoinDropped = 2;

    public GameObject projectile;
    public Transform projectileSpawnPoint;
    private float timeToShoot = 1f;
    private float timeSinceShot;
    private bool shootVertically = true;

    Rigidbody2D rb;

    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        goingRight = false;
        goingUp = true;
        unitCenterPosition = transform.position;
        timeSinceShot = timeToShoot;
        moveDirX = -1;

        if (canBob)
        {
            moveDirY = 1;
        }
    }

    void Start()
    {
        maxHealth = 1;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (timeSinceShot <= 0)
        {
            timeSinceShot = timeToShoot;
            animator.SetTrigger("isShooting");
            Attack();
        }
        else
        {
            timeSinceShot -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    public void Patrol()
    {
        rb.velocity = new Vector2(moveDirX * flyingSpeed, moveDirY * flyingSpeed / 2);
        if (goingRight && transform.position.x > (unitCenterPosition.x + patrolRadiusX))
        {
            moveDirX *= -1;
            goingRight = false;
            Flip();
        }
        else if (!goingRight && transform.position.x < (unitCenterPosition.x - patrolRadiusX))
        {
            moveDirX *= -1;
            goingRight = true;
            Flip();
        }
        if (canBob)
        {
            if (goingUp && transform.position.y > (unitCenterPosition.y + patrolRadiusY))
            {
                moveDirY *= -1;
                goingUp = false;
            } else if (!goingUp && transform.position.y < (unitCenterPosition.y - patrolRadiusY))
            {
                moveDirY *= -1;
                goingUp = true;
            }
        }

    }

    public override void Attack()
    {
        if(shootVertically)
        {
            shootVertically = false;
            Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(0f, 0f, -45f));
            Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(0f, 0f, 135f));

        } else
        {
            shootVertically = true;
            Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(0f, 0f, 45f));
            Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(0f, 0f, -135f));
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

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    public override void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        DropLoot();
        Destroy(gameObject);
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(unitCenterPosition, patrolRadiusX);
        Gizmos.DrawWireSphere(unitCenterPosition, patrolRadiusY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            moveDirX *= -1;
            goingRight = !goingRight;
            Flip();
        }
    }
}
