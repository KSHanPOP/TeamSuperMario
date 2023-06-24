using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    private readonly int hashIsIdle = Animator.StringToHash("IsIdle");
    private readonly int hashIsGround = Animator.StringToHash("IsGround");
    private readonly int hashTryStop = Animator.StringToHash("TryStop");
    private readonly int hashSpeed = Animator.StringToHash("Speed");        

    private GroundChecker groundChecker;
    private MoveController movement;
    private Rigidbody2D rigidbody2;

    [SerializeField]
    private float minVelocityToMove = 0.01f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private PlayerState playerState;

    [SerializeField]
    MarioState startState = MarioState.Small;

    [SerializeField]
    private float runAnimationSpeed = 1f;

    private bool isTryStop;  

    void Awake()
    {
        groundChecker = GetComponent<GroundChecker>();
        movement = GetComponent<MoveController>();
        rigidbody2 = GetComponent<Rigidbody2D>();

        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Player;
    }
    private void Start()
    {        
        playerState.Animator = animator;
        playerState.SetStartState(startState);
    }
    void Update()
    {
        animator.SetBool(hashIsGround, groundChecker.IsGround());
        animator.SetBool(hashTryStop, isTryStop = rigidbody2.velocity.x * movement.directionX < 0);
        animator.SetFloat(hashSpeed, rigidbody2.velocity.x * runAnimationSpeed);
        SetMoveAnimation();     
    }
    private void SetMoveAnimation()
    {
        float velocityX = rigidbody2.velocity.x;

        bool isIdle = minVelocityToMove > Mathf.Abs(velocityX);

        animator.SetBool(hashIsIdle, isIdle);

        if (isIdle)
            return;

        if (!groundChecker.IsGround())
            return;

        spriteRenderer.flipX = velocityX < 0;        
    }
    public void TryBackJump()
    {        
        if(isTryStop)
            spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void EatMushroom()
    {
        playerState.CurrState.EatMushroom();
    }
    public void EatFireFlower()
    {   
        playerState.CurrState.EatFireFlower();
    }
    public void Hit()
    {   
        playerState.CurrState.Hit();
    }
    public void OnTransformationComplete()
    {   
        playerState.CurrState.OnTransformationComplete();        
    }
}

public enum MarioState
{
    None = -1,
    Small = 0,
    Big,
    Fire,
}

