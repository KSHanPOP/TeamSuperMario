using UnityEngine;
using GMTK.PlatformerToolkit;
using UnityEngine.Playables;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    private readonly int hashIsIdle = Animator.StringToHash("IsIdle");
    private readonly int hashIsGround = Animator.StringToHash("IsGround");
    private readonly int hashTryStop = Animator.StringToHash("TryStop");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashEatMushroom = Animator.StringToHash("EatMushroom");
    private readonly int hashEatFireFlower = Animator.StringToHash("EatFireFlower");
    private readonly int hashIsTransformationCompleted = Animator.StringToHash("IsTransformationCompleted");

    private characterGround characterGround;
    private characterMovement characterMovement;
    private Rigidbody2D rigidbody2;

    [SerializeField]
    private float minVelocityToMove = 0.01f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private PlayerState playerState;

    [SerializeField]
    MarioState startState = MarioState.Small;

    void Awake()
    {
        characterGround = GetComponent<characterGround>();
        characterMovement = GetComponent<characterMovement>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {        
        playerState.Animator = animator;
        playerState.SetStartState(startState);
    }
    void Update()
    {
        animator.SetBool(hashIsGround, characterGround.GetOnGround());
        animator.SetBool(hashTryStop, rigidbody2.velocity.x * characterMovement.directionX < 0);
        animator.SetFloat(hashSpeed, rigidbody2.velocity.x * 0.2f);
        SetMoveAnimation();

        TestCodes();
    }
    private void SetMoveAnimation()
    {
        float velocityX = rigidbody2.velocity.x;

        bool isIdle = minVelocityToMove > Mathf.Abs(velocityX);

        animator.SetBool(hashIsIdle, isIdle);

        if (isIdle)
            return;

        if (!characterGround.GetOnGround())
            return;

        spriteRenderer.flipX = velocityX < 0;        
    }
    public void BackJump()
    {        
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void TestCodes()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            EatMushroom();            
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Hit();            
        }
    }
    public void EatMushroom()
    {
        animator.SetTrigger(hashEatMushroom);
        playerState.CurrState.EatMushroom();
    }
    public void EatFireFlower()
    {
        animator.SetTrigger(hashEatFireFlower);
        playerState.CurrState.EatFireFlower();
    }
    public void Hit()
    {
        animator.SetTrigger(hashHit);
        playerState.CurrState.Hit();
    }
    public void OnTransformationComplete()
    {   
        animator.SetTrigger(hashIsTransformationCompleted);
        playerState.CurrState.OnTransformationComplete();
        playerState.nextState.Enter();
    }
}

public enum MarioState
{
    None = -1,
    Small = 0,
    Big,
    Fire,
}

