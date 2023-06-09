using UnityEngine;
using GMTK.PlatformerToolkit;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    private int hashIsIdle = Animator.StringToHash("IsIdle");
    private int hashIsGround = Animator.StringToHash("IsGround");    
    private int hashTryStop = Animator.StringToHash("TryStop");
    private int hashSpeed = Animator.StringToHash("Speed");
    private int hashHit = Animator.StringToHash("Hit");
    private int hashEatMushroom = Animator.StringToHash("EatMushroom");
    private int hahsEatFireFlower = Animator.StringToHash("EatFireFlower");

    private characterGround characterGround;
    private characterMovement characterMovement;
    private Rigidbody2D rigidbody2;

    [SerializeField]
    private float minVelocity = 0.1f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private RuntimeAnimatorController marioSmall;

    [SerializeField]
    private RuntimeAnimatorController marioBigOverride;

    [SerializeField]
    private RuntimeAnimatorController marioFireOverride;


    void Start()
    {
        characterGround = GetComponent<characterGround>();
        characterMovement = GetComponent<characterMovement>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        animator.SetBool(hashIsGround, characterGround.GetOnGround());
        animator.SetBool(hashTryStop, rigidbody2.velocity.x * characterMovement.directionX < 0);
        animator.SetFloat(hashSpeed, rigidbody2.velocity.x * 0.2f);
        SetMoveAnimation();

        if(Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger(hashEatMushroom);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger(hashHit);
        }
    }
    private void SetMoveAnimation()
    {
        float velocityX = rigidbody2.velocity.x;

        bool isIdle = minVelocity > Mathf.Abs(velocityX);
        
        animator.SetBool(hashIsIdle, isIdle);

        if (isIdle)
            return;

        if (!characterGround.GetOnGround())
            return;

        spriteRenderer.flipX = velocityX < 0;
    }

    public void Hit()
    {

    }
}
