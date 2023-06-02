using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected Rigidbody2D rb;

    Vector2 v2;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void DoMove()
    {
        rb.velocity = Vector2.left * speed;
    }
}

