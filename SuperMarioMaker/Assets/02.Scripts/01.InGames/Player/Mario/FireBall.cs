using UnityEngine;

public class FireBall : MonoBehaviour
{
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

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

    private bool isFire = false;


    private Vector2 startPosForTest;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        startPosForTest = transform.position;
    }

    private void FixedUpdate()
    {
        if (!isFire)
            return;

        velocity.x = dir * speed;        
        velocity.y = Mathf.Clamp(rb.velocity.y, -maxFallSpeed, float.MaxValue);

        rb.velocity = velocity;
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
        animator.SetFloat(hashSpeed, -1.5f);
    }
    public void SetDirLeft()
    {
        dir = -1;
        spriteRenderer.flipX = false;
        animator.SetFloat(hashSpeed, 1.5f);
    }

    public void Stop()
    {
        isFire = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetDirRight();
            Fire();
        }
    }

    private void Hit()
    {
        animator.SetTrigger(hashHit);

        Stop();
    }

    private void Bounce()
    {   
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }
    public void DestroyFireball()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;

        if (go.CompareTag("Platform"))
            Bounce();

        if (go.CompareTag("Monster"))
        {
            go.GetComponent<IShakeable>().Shake(Vector2.one);
            Hit();
        }            
    }
}
