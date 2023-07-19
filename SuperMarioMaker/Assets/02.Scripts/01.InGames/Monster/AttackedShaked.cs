using UnityEngine;

public class AttackedShaked : MonoBehaviour, IShakeable
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private float speedMultiplier = 1.5f;

    [SerializeField]
    protected float shakeForce = 30f;

    [SerializeField]
    protected int fallingLayer;

    private int hashSpeed = Animator.StringToHash("Speed");

    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void SetAnimation()
    {
        animator.SetFloat(hashSpeed, speedMultiplier);
        spriteRenderer.flipY = true;
    }

    public virtual void Shake(Vector2 _)
    {
        SetAnimation();

        GetComponent<ObjectMove>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * shakeForce, ForceMode2D.Impulse);

        gameObject.layer = fallingLayer;

        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.MonsterDie;        
    }
}
