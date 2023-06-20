using GMTK.PlatformerToolkit;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected readonly int hashTransformation = Animator.StringToHash("Transformation");
    protected readonly int hashIsTransformationCompleted = Animator.StringToHash("IsTransformationCompleted");

    protected PlayerState playerState;

    protected SpriteRenderer sprite;

    protected Transform spriteTransform;

    protected Rigidbody2D rb;

    protected Vector2 velocityBeforeTransformation;

    protected float gravityScaleBeforeTransformation;

    protected BoxCollider2D playerCollider;

    protected BoxCollider2D playerTrigger;

    protected Vector2 smallColliderSize = new Vector2(0.75f, 0.95f);

    protected Vector2 smallColliderOffset = new Vector2(0f, 0);

    protected Vector2 smallTriggerSize = new Vector2(0.73f, 0.05f);

    protected float smallBlockDetectLength = 0.5f;

    protected Vector2 bigColliderSize = new Vector2(1f, 1.95f);

    protected Vector2 bigColliderOffset = new Vector2(0f, 0.5f);

    protected Vector2 bigTriggerSize = new Vector2(0.97f, 0.05f);

    protected float bigBlockDetectLength = 1.5f;

    [SerializeField]
    protected RuntimeAnimatorController controller;

    protected virtual void Awake()
    {        
        playerState = GetComponent<PlayerState>();
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = transform.parent.GetComponentInChildren<SpriteRenderer>();        
        spriteTransform = sprite.transform;

        SetColliders();
    }
    private void SetColliders()
    {
        playerCollider = playerState.playerCollider;

        playerTrigger = playerState.playerTrigger;
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

    public virtual void StartTransformation()
    {
        playerState.IsAttckable = false;

        movementLimiter.instance.CharacterCanMove = false;

        velocityBeforeTransformation = rb.velocity;
        gravityScaleBeforeTransformation = rb.gravityScale;

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        playerState.SetInvincibleLayer();
    }
    public virtual void OnTransformationComplete()
    {
        playerState.IsAttckable = true;

        movementLimiter.instance.CharacterCanMove = true;

        rb.velocity = velocityBeforeTransformation;
        rb.gravityScale = gravityScaleBeforeTransformation;

        playerState.Animator.SetTrigger(hashIsTransformationCompleted);

        playerState.nextState.Enter();
    }

    protected virtual void SetSmallCollider()
    {
        playerCollider.size = smallColliderSize;
        playerCollider.offset = smallColliderOffset;
        playerTrigger.size = smallTriggerSize;
        playerState.BlockDetectLength = smallBlockDetectLength;
    }
    protected virtual void SetBigCollider()
    {
        playerCollider.size = bigColliderSize;
        playerCollider.offset = bigColliderOffset;
        playerTrigger.size = bigTriggerSize;
        playerState.BlockDetectLength = bigBlockDetectLength;
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
