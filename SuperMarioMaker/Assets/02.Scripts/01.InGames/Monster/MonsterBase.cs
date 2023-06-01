using UnityEngine;
public abstract class MonsterBase : MonoBehaviour
{
    protected float speed;
    protected Rigidbody2D rb;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void DoMove()
    {
        rb.velocity = Vector2.left * speed;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") ||
            collision.gameObject.CompareTag("Player"))
        {
            speed *= -1;
            DoMove();
        }
    }
}
