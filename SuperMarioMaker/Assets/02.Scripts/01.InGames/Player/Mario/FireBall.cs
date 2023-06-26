using UnityEngine;

public class FireBall : MonoBehaviour
{
    private readonly int hashHit = Animator.StringToHash("Hit");

    private Rigidbody2D rb;

    private float dir = 1;

    private Vector2 velocity = Vector2.right;    

    [SerializeField]
    private float speed;

    [SerializeField]
    private float bounceForce;

    [SerializeField]
    private float maxFallSpeed;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private float lifeTime = 5f;

    private bool isFire = false;

    [SerializeField]
    private LayerMask platformLayer;

    private Vector2 startPosForTest;

    private bool isHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        startPosForTest = transform.position;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (!isFire)
            return;

        if (isHit)
            return;

        velocity.x = dir * speed;        
        velocity.y = Mathf.Clamp(rb.velocity.y, -maxFallSpeed, float.MaxValue);

        rb.velocity = velocity;
    }

    private void Update()
    {
        if (isHit)
            return;        

        if (Physics2D.Raycast(transform.position, Vector2.right * dir, 0.3f, platformLayer))
        {
            Hit();
            transform.position += dir * 0.2f * Vector3.right;
        }
    }
    public void Fire()
    {
        transform.position = startPosForTest;

        isFire = true;
        velocity.x = dir * speed;
        velocity.y = - maxFallSpeed;

        rb.velocity = velocity;
    }
  
    public void SetDirRight()
    {
        dir = 1;
        spriteRenderer.flipX = true;        
    }
    public void SetDirLeft()
    {
        dir = -1;
        spriteRenderer.flipX = false;        
    }

    public void FireWithDirection(bool left)
    {
        if (left)
        {
            SetDirLeft();
        }
        else
            SetDirRight();

        Fire();
    }

    public void Stop()
    {
        isFire = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }
    private void Hit()
    {
        boxCollider.enabled = false;

        isHit = true;

        animator.SetTrigger(hashHit);

        Stop();
    }

    private void Bounce()
    {   
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }
    public void DestroyFireball()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit)
            return;

        if (collision.gameObject.CompareTag("Platform"))
            Bounce();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit)
            return;

        if (collision.CompareTag("Monster"))
        {
            collision.GetComponent<IShakeable>().Shake(Vector2.one);
            Hit();
        }
    }
}
