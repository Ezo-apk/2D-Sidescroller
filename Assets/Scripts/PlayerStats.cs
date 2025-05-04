using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    public int money = 0;
    public int maxHealth = 3;
    public int currentHealth;
    public float iframes = 0.5f;
    public float iframesCooldown = 0;
    public bool gotHit = false;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public TextMeshProUGUI moneyDisplay;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        moneyDisplay.text = money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; ++i)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if(currentHealth <= 0)
        {
            PlayerDeath();
        }

        if (iframesCooldown > 0)
        {
            iframesCooldown -= Time.deltaTime;
        }

        if(this.transform.position.x > 455 && this.transform.position.y >= 33)
        {
            SceneManager.LoadScene("Win Screen");
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (iframesCooldown <= 0)
            {
                TakeDamage(1);
                //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false);
            } else
            {
                //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            money++;
            moneyDisplay.text = money.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "HealthRefill")
        {
            if (currentHealth < maxHealth)
            {
                currentHealth++;
                Mathf.Clamp(currentHealth, 0, maxHealth);
            }
        }

        if (collision.gameObject.tag == "HealthUpgrade")
        {
            maxHealth++;
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int dmg)
    {
            currentHealth -= dmg;
            currentHealth = currentHealth < 0 ? 0 : currentHealth;
            iframesCooldown = iframes;
    }


    void PlayerDeath()
    {
        Debug.Log("Player Kicked the bucket");
        SceneManager.LoadScene("Death Screen");
    }

}
