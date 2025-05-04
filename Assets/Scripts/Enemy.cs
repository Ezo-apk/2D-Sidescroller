using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;


    public virtual void TakeDamage(int dmg) {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Attack() { }

    public virtual void Die() { }

}
