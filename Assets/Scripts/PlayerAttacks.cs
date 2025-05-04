using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{

    public bool unlockedBow;
    public float attackCooldown;
    public float timeSinceAttack;
    public int meleeDmg = 1;
    public int rangedDmg;

    public Transform meleeAreaCenter;
    public Transform arrowSource;
    public LayerMask isEnemy;
    public LayerMask bullets;
    public float meleeRange = 1;
    public float bowRange;

    public GameObject Arrow;

    public Animator animator;

    public Tooltips tooltip;

    // Start is called before the first frame update
    void Start()
    {
        unlockedBow = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (timeSinceAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                MeleeAttack();
                timeSinceAttack = attackCooldown;
            }

            if (unlockedBow && Input.GetKeyUp(KeyCode.L))
            {
                RangedAttack();
                timeSinceAttack = attackCooldown;
            }
        } else {
            timeSinceAttack -= Time.deltaTime;
        }
    }

    void MeleeAttack()
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(meleeAreaCenter.position, meleeRange, isEnemy);
        Collider2D[] bulletsToDestroy = Physics2D.OverlapCircleAll(meleeAreaCenter.position, meleeRange, bullets);

        for (int i = 0; i < enemiesToHit.Length; ++i)
        {
            enemiesToHit[i].GetComponent<Enemy>().TakeDamage(meleeDmg);
        }

        for (int i = 0; i < bulletsToDestroy.Length; ++i)
        {
            bulletsToDestroy[i].GetComponent<EnemyBullet>().getDestroyed();
        }


        animator.SetTrigger("isAttacking");
    }

    void RangedAttack()
    {
        int rotOffset = Mathf.RoundToInt(transform.rotation.y) * 180;
        Instantiate(Arrow, arrowSource.position, Quaternion.Euler(0f, rotOffset, 0f));
        animator.SetTrigger("isShooting");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "BowUnlock")
        {
            unlockedBow = true;
            tooltip.EnableSthootingTip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeAreaCenter == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(meleeAreaCenter.position, meleeRange);
    }

}
