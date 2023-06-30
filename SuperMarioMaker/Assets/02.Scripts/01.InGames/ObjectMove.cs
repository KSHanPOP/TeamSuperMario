using UnityEngine;
public class ObjectMove : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected float dir = -1;

    protected float velocityX;

    protected Rigidbody2D rb;

    public bool UseSpriteFlip;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    Vector2 newVelocity = Vector2.zero;

    private bool isStop = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVelocityX();
    }

    protected virtual void SetVelocityX()
    {
        velocityX = dir * speed;
    }
    private void Update()
    {
        DoMove();        
    }

    public virtual void DoMove()
    {
        if (isStop)
            return;

        newVelocity.x = velocityX;
        newVelocity.y = rb.velocity.y;

        rb.velocity = newVelocity;
    }

    public virtual void ReverseMoveDir()
    {   
        dir = -dir;
        velocityX = dir * speed;

        if (UseSpriteFlip)
            spriteRenderer.flipX = dir > 0;
    }

    public virtual void ChageSpeed(float speed)
    {
        this.speed = speed;
        velocityX = dir * speed;
    }

    public virtual void Stop()
    {
        isStop = true;
    }    

    public float GetDir() => dir;
}