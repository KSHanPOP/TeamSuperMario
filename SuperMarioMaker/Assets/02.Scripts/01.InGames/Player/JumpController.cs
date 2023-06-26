using UnityEngine;
using UnityEngine.InputSystem;

public class JumpController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D body;
    private GroundChecker ground;

    //[SerializeField] private float jumpForce;
    //private Vector2 v2JumpForce;

    [SerializeField]
    private float gravityScale;

    [SerializeField]
    private float maxFallSpeed;

    public float MaxFallSpeed { set { maxFallSpeed = value; } }

    [SerializeField]
    private float maxJumpSpeed;

    [SerializeField, Range(0f, 1f)]
    private float minJumpPowerReduction;

    [SerializeField, Range(0f, 1f)]
    private float maxJumpPowerReduction;

    [SerializeField]
    private float maxJumpkeyHoldTime = 0.5f;

    private float inversMaxJumpkeyHoldTime;

    private float jumpTimeCounter = 0f;

    [SerializeField]
    private float jumpBuffer;    
    private float lastJumpBufferInputTime;

    private bool jumpCutBuffer;

    [SerializeField]
    private float jumpVelocity;

    private float jumpKeyValue;

    private MovementLimmiter limmiter;
    
    public bool IsAttackableBlock { get; set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundChecker>();         
        body.gravityScale = gravityScale;

        inversMaxJumpkeyHoldTime = 1 / maxJumpkeyHoldTime;

        InitSetting();
    }
    private void Start()
    {
        limmiter = MovementLimmiter.instance;
    }
    private void InitSetting()
    {
        //v2JumpForce = Vector2.up * jumpForce;        
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
        if (!limmiter.CharacterCanMove)
            return;

        if (ground.IsGround() && (Time.time - lastJumpBufferInputTime) < jumpBuffer)
        {
            DoJump();
        }

        TryCutJump();
    }
    public void DoJump()
    {
        jumpTimeCounter = 0f;

        IsAttackableBlock = true;

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

        var jumpPowerReduction = Mathf.Lerp(minJumpPowerReduction, maxJumpPowerReduction, jumpTimeCounter * inversMaxJumpkeyHoldTime);
        
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * jumpPowerReduction);
        jumpCutBuffer = false;        
    }

    void Update()
    {
        jumpTimeCounter += Time.deltaTime;

        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, maxFallSpeed, maxJumpSpeed));
    }
}
