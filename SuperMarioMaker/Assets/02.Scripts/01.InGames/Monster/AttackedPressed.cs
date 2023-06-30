using UnityEngine;

public class AttackedPressed : MonoBehaviour, IPressable
{
    private Animator animator;
    private readonly int hashPressed = Animator.StringToHash("Pressed");

    private Rigidbody2D rb;

    public float lifeTime = 0.1f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void Press()
    {
        animator.SetTrigger(hashPressed);
        GetComponent<Collider2D>().enabled = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        GetComponent<ObjectMove>().Stop();
        Invoke(nameof(Attacked), lifeTime);
    }       
    protected virtual void Attacked()
    {   
        Destroy(gameObject);
    }
}
