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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        velocity.x = dir * speed;
        velocity.y = rb.velocity.y < maxFallSpeed ? rb.velocity.y : maxFallSpeed;

        rb.velocity = velocity;
    }
    public void SetDirRight()
    {
        dir = 1;
    }
    public void SetDirLeft()
    {
        dir = -1;
    }

    private void Hit()
    {
        animator.SetTrigger(hashHit);
    }

    private void Bounce()
    {
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }
    public void DestroyFireball()
    {
        Destroy(gameObject);
    }
}