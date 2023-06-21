using UnityEngine;

public class AttackedShaked : MonoBehaviour, IShakeable
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private float speedMultiplier = 1.5f;

    [SerializeField]
    private float shakeForce = 30f;

    [SerializeField]
    private int fallingLayer;

    private int hashSpeed = Animator.StringToHash("Speed");

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shake()
    {
        Logger.Debug("shaked!");

        animator.SetFloat(hashSpeed, speedMultiplier);

        GetComponent<ObjectMove>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * shakeForce, ForceMode2D.Impulse);

        gameObject.layer = fallingLayer;        

        spriteRenderer.flipY = true;
    }

}
