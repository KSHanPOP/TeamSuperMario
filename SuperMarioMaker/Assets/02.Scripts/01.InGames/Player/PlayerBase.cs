using GMTK.PlatformerToolkit;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected PlayerState playerState;

    protected BlockDetector detector;

    protected SpriteRenderer sprite;

    protected Transform spriteTransform;

    protected Transform spritePivotTransform;

    protected PlayerAnimation playerAnimation;

    protected Rigidbody2D rb;

    protected Vector2 velocityBeforeTransformation;

    protected float gravityScaleBeforeTransformation;

    protected BoxCollider2D playerCollider;

    protected BoxCollider2D playerTrigger;

    protected Vector2 smallColliderSize = new Vector2(0.75f, 0.95f);

    protected Vector2 smallColliderOffset = new Vector2(0f, 0);

    protected float smallBlockDetectLength = 0.5f;

    protected Vector2 bigColliderSize = new Vector2(0.75f, 1.95f);

    protected Vector2 bigColliderOffset = new Vector2(0f, 0.5f);

    protected float bigBlockDetectLength = 1.5f;

    [SerializeField]
    protected RuntimeAnimatorController controller;

    public RuntimeAnimatorController Controller { get { return controller; } }

    protected virtual void Awake()
    {
        playerState = GetComponent<PlayerState>();
        detector = GetComponent<BlockDetector>();
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
        spriteTransform = sprite.transform;
        spritePivotTransform = spriteTransform.parent;
        playerAnimation = transform.parent.GetComponent<PlayerAnimation>();

        SetCollider();
    }
    private void SetCollider()
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

        playerAnimation.enabled = false;
        playerState.Animator.speed = 0f;

        playerState.SetInvincibleLayer();
    }
    public virtual void OnTransformationComplete()
    {
        playerState.IsAttckable = true;

        movementLimiter.instance.CharacterCanMove = true;

        rb.velocity = velocityBeforeTransformation;
        rb.gravityScale = gravityScaleBeforeTransformation;

        playerAnimation.enabled = true;
        playerState.Animator.speed = 1f;
    }

    protected virtual void SetSmallCollider()
    {
        playerCollider.size = smallColliderSize;
        playerCollider.offset = smallColliderOffset;
        CopyColliderToTrigger();

        detector.BlockDetectLength = smallBlockDetectLength;
    }
    protected virtual void SetBigCollider()
    {
        playerCollider.size = bigColliderSize;
        playerCollider.offset = bigColliderOffset;
        CopyColliderToTrigger();

        detector.BlockDetectLength = bigBlockDetectLength;
    }
    protected virtual void CopyColliderToTrigger()
    {
        playerTrigger.size = playerCollider.size;
        playerTrigger.offset = playerCollider.offset;
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
