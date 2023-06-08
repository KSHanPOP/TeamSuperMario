using GMTK.PlatformerToolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D body;
    private characterGround ground;

    //[SerializeField] private float jumpForce;
    //private Vector2 v2JumpForce;

    [SerializeField]
    private float gravityScale;

    [SerializeField]
    private float maxFallSpeed;
    [SerializeField]
    private float maxJumpSpeed;

    [SerializeField]
    private float jumpCutForce;
    private float divJumpCutForce;

    [SerializeField]
    private float jumpBuffer;    
    private float lastJumpBufferInputTime;

    private bool jumpCutBuffer;

    [SerializeField]
    private float jumpVelocity;

    private float jumpKeyValue;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<characterGround>();         
        body.gravityScale = gravityScale;
        InitSetting();
    }
    private void InitSetting()
    {
        //v2JumpForce = Vector2.up * jumpForce;
        divJumpCutForce = 1 / jumpCutForce;
        maxFallSpeed *= -1;
        lastJumpBufferInputTime = -jumpBuffer;
    }

    public void TryJump(InputAction.CallbackContext context)
    {
        jumpKeyValue = context.ReadValue<float>();

        if(jumpKeyValue == 1)
        {
            lastJumpBufferInputTime = Time.time;            
        }   
    }

    private void FixedUpdate()
    {
        if (ground.GetOnGround() && (Time.time - lastJumpBufferInputTime) < jumpBuffer)
        {
            DoJump();
        }

        TryCutJump();
    }
    public void DoJump()
    {
        //body.velocity = new Vector2(body.velocity.x, 0);
        //body.AddForce(v2JumpForce, ForceMode2D.Force);

        body.velocity = new Vector2(body.velocity.x, jumpVelocity);

        lastJumpBufferInputTime = -jumpBuffer;
        jumpCutBuffer = true;
    }
    private void TryCutJump()
    {
        if (!jumpCutBuffer)
            return;

        if (jumpKeyValue != 0)
            return;

        if (body.velocity.y <= 0)
            return;
        
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * divJumpCutForce);
        jumpCutBuffer = false;        
    }

    void Update()
    {
        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, maxFallSpeed, maxJumpSpeed));        
    }
}
