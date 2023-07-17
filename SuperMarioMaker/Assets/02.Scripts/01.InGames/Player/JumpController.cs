using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

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
    private float minJumpPower;

    [SerializeField, Range(0f, 1f)]
    private float maxJumpPower;

    [SerializeField]
    private float maxJumpkeyHoldTime = 0.5f;

    [SerializeField]
    private PlayerState playerState;

    private float jumpKeyHoldTimeCounter = 0f;

    [SerializeField]
    private float jumpBufferDuration;    
    private float lastJumpBufferInputTime;

    private bool canCutableJumpPower;

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
        lastJumpBufferInputTime = -jumpBufferDuration;
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

        //플레이어가 땅에 닿아있는지 && 점프 키 버퍼가 남아있는지 체크.
        if (ground.IsGround() && (Time.time - lastJumpBufferInputTime) < jumpBufferDuration)
        {
            GroundJump();
        }

        TryCutJump();
    }
    public void DoJump()
    {
        //점프 시작 시 키 입력 시간 카운터 초기화        
        jumpKeyHoldTimeCounter = 0f;

        IsAttackableBlock = true;

        body.velocity = new Vector2(body.velocity.x, jumpVelocity);

        lastJumpBufferInputTime = -jumpBufferDuration;

        //점프력 감소는 점프 한번당 한번씩만 일어나도록 함
        canCutableJumpPower = true;
    }

    public void GroundJump()
    {
        playerState.CurrState.PlayJumpSound();
        DoJump();
    }


    public void MonsterPressJump()
    {
        SoundManager.Instance.PlaySFX("Stomp");
        DoJump();
    }

    private void TryCutJump()
    {
        //점프력 감소가 불가능상태시 리턴
        if (!canCutableJumpPower)
            return;

        //점프 키가 눌린 상태면 리턴
        if (jumpKeyValue != 0)
            return;

        //플레이어가 낙하 중에는 리턴
        if (body.velocity.y <= 0)
            return;

        //점프키를 누른 시간이 작을수록 점프력을 크게 감소. 누른 시간이 길수록 약하게 감소.
        var jumpPowerReduction = Mathf.Lerp(minJumpPower, maxJumpPower, jumpKeyHoldTimeCounter / maxJumpkeyHoldTime);        

        //현재 플레이어에 점프속도를 위 결과에 따라 감소시킴.
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * jumpPowerReduction);

        //점프력 감소 불가능하게 처리.
        canCutableJumpPower = false;        
    }

    void Update()
    {
        jumpKeyHoldTimeCounter += Time.deltaTime;

        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, maxFallSpeed, maxJumpSpeed));
    }
}
