using UnityEngine;
public class ObjectMove : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected float dir = -1;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        dir *= speed;

        DoMove();
    }

    private void Update()
    {
        DoMove();
    }

    public virtual void DoMove()
    {
        rb.velocity = new Vector2(dir, rb.velocity.y);
    }

    public virtual void ChangeMoveDir()
    {
        dir = -dir;
    }

    public virtual void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}

