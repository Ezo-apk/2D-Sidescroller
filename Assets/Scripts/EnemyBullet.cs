using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed;
    public float timeToLive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timeToLive -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Vector3 shootingDir = new Vector3(1f, 1f, 0f);
        transform.Translate(shootingDir * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    public void getDestroyed()
    {
        Destroy(gameObject);
    }

}
