using UnityEngine;
using GMTK.PlatformerToolkit;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;

    private int hashIsIdle = Animator.StringToHash("IsIdle");
    private int hashIsGround = Animator.StringToHash("IsGround");
    //private int hashIsMoving = Animator.StringToHash("IsMoving");
    private int hashTryStop = Animator.StringToHash("TryStop");
    private int hashSpeed = Animator.StringToHash("Speed");

    private characterGround characterGround;
    private characterMovement characterMovement;
    private Rigidbody2D rigidbody2;

    [SerializeField]
    private float minVelocity = 0.1f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;    
    
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
        //float currVelocityAndInput = rigidbody2.velocity.x * characterMovement.directionX;
        //animator.SetBool(hashIsMoving, currVelocityAndInput > 0);
    }
    private void SetMoveAnimation()
    {
        float velocityX = rigidbody2.velocity.x;        

        bool isIdle = minVelocity > Mathf.Abs(velocityX);

        //animator.SetBool(hashIsMoving, !isIdle);
        animator.SetBool(hashIsIdle, isIdle); 

        if (isIdle)
            return;

        if (!characterGround.GetOnGround())
            return;

        spriteRenderer.flipX = velocityX < 0;
    }
}
