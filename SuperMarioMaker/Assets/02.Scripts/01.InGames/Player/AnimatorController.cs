using UnityEngine;
using GMTK.PlatformerToolkit;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;

    private int hashIsGround = Animator.StringToHash("IsGround");
    private int hashIsMoing = Animator.StringToHash("IsMoving");

    private characterGround characterGround;
    private characterMovement characterMovement;
    private Rigidbody2D rigidbody2;

    [SerializeField]
    private float minVelocity = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        characterGround = GetComponent<characterGround>();
        characterMovement = GetComponent<characterMovement>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(hashIsGround, characterGround.GetOnGround());
        animator.SetBool(hashIsMoing, characterMovement.directionX != 0);
        //SetScaleForDir();
    }
    private void SetScaleForDir()
    {
        float velocityX = rigidbody2.velocity.x;

        if (minVelocity > velocityX && velocityX > -minVelocity)
            velocityX = 0;

        float dir = 1;        

        if (velocityX > 0) dir = 1;
        if (velocityX < 0) dir = -1;

        transform.localScale = new Vector3(dir, 1, 1); 
    }
}
