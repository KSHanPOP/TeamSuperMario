using UnityEngine;

public class TurtleDie : MonoBehaviour, IShakeable
{
    private readonly int hashDie = Animator.StringToHash("Die");

    [SerializeField]
    Animator animator;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private float shakeForce = 30f;

    [SerializeField]
    private int fallingLayer;    

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shake(Vector2 _)
    {
        Logger.Debug("shaked!");

        animator.SetTrigger(hashDie);

        GetComponent<ObjectMove>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * shakeForce, ForceMode2D.Impulse);

        gameObject.layer = fallingLayer;

        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.MonsterDie;
        spriteRenderer.flipY = true;
    }
}
