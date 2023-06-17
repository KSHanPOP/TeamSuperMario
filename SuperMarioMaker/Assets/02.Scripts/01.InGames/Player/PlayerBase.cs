using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected PlayerState playerState;

    protected Rigidbody2D rb;

    protected BoxCollider2D playerCollider;

    protected BoxCollider2D playerTrigger;

    protected Vector2 smallColliderSize = new Vector2(0.75f, 1);

    protected Vector2 smallColliderOffset = new Vector2(0f, 0f);

    protected Vector2 smallTriggerSize = new Vector2(0.73f, 0.05f);

    protected Vector2 bigColliderSize = new Vector2(1f, 2f);

    protected Vector2 bigColliderOffset = new Vector2(0f, 0.5f);

    protected Vector2 bigTriggerSize = new Vector2(0.97f, 0.05f);

    [SerializeField]
    protected RuntimeAnimatorController controller;    
    protected virtual void Awake()
    {        
        playerState = GetComponent<PlayerState>();
        rb = GetComponentInParent<Rigidbody2D>();
        SetColliders();
    }
    private void SetColliders()
    {
        var colliders = GetComponentsInParent<BoxCollider2D>();

        foreach(var collider in colliders)
        {
            if (collider.isTrigger)
                playerTrigger = collider;
            else
                playerCollider = collider;
        }
    }

    public virtual void Enter()
    {
        playerState.CurrState = this;
        playerState.Animator.runtimeAnimatorController = controller;
    }
    public virtual void EatMushroom()
    {

    }
    public virtual void EatFireFlower()
    {

    }
    public virtual void Hit()
    {

    }
    protected virtual void SetSmallCollider()
    {
        playerCollider.size = smallColliderSize;
        playerCollider.offset = smallColliderOffset;
        playerTrigger.size = smallTriggerSize;
    }
    protected virtual void SetBigCollider()
    {
        playerCollider.size = bigColliderSize;
        playerCollider.offset = bigColliderOffset;
        playerTrigger.size = bigTriggerSize;
    }

    //public virtual void PlayerUpdate()
    //{

    //}
    //public virtual void PlayerOnTriggerEnter()
    //{

    //}

    //public virtual void PlayerOnTriggerStay()
    //{

    //}

    //public virtual void PlayerOnTriggerExit()
    //{

    //}
}
