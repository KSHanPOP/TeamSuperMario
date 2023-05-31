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
    }
}
