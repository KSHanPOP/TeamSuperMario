using UnityEngine;
public class ObjectMove : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected float dir = -1;

    protected float velocityX;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        velocityX = dir * speed;        
    }

    private void Update()
    {
        DoMove();        
    }

    public virtual void DoMove()
    {
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    public virtual void ChangeMoveDir()
    {   
        dir = -dir;
    }

    public virtual void ChageSpeed(float speed)
    {
        this.speed = speed;
        velocityX = dir * speed;
    }

    public virtual void Stop()
    {
        rb.velocity = Vector2.zero;
        velocityX = 0f;
    }
}

