using UnityEngine;
using GMTK.PlatformerToolkit;

public class FireBall : MonoBehaviour
{
    private readonly int hashHit = Animator.StringToHash("Hit");

    private Rigidbody2D rb;

    private float dir = 1;

    private Vector2 velocity = Vector2.right;

    private characterGround characterGround;

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
        characterGround = GetComponent<characterGround>();
    }

    private void FixedUpdate()
    {
        velocity.x = dir * speed;
        velocity.y = rb.velocity.y < maxFallSpeed ? rb.velocity.y : maxFallSpeed;

        rb.velocity = velocity;

        if (characterGround.GetOnGround())
            Bounce();
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
