using UnityEngine;

public class AttackedPressed : MonoBehaviour, IAttackable
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
    public virtual void OnAttack()
    {
        animator.SetTrigger(hashPressed);
        var colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        rb.gravityScale = 0;
        GetComponent<MonsterMove>().Stop();
        Invoke(nameof(Attacked), lifeTime);
    }       
    protected virtual void Attacked()
    {   
        Destroy(gameObject);
    }
}
