using UnityEngine;

public class Projectile : MonoBehaviour
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
        float direction = transform.rotation.y;
        direction = direction == 0 ? 1 : -1;
        transform.Translate(transform.right * speed * direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }
    }


}
