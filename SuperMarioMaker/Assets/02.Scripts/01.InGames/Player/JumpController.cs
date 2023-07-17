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

        //�÷��̾ ���� ����ִ��� && ���� Ű ���۰� �����ִ��� üũ.
        if (ground.IsGround() && (Time.time - lastJumpBufferInputTime) < jumpBufferDuration)
        {
            GroundJump();
        }

        TryCutJump();
    }
    public void DoJump()
    {
        //���� ���� �� Ű �Է� �ð� ī���� �ʱ�ȭ        
        jumpKeyHoldTimeCounter = 0f;

        IsAttackableBlock = true;

        body.velocity = new Vector2(body.velocity.x, jumpVelocity);

        lastJumpBufferInputTime = -jumpBufferDuration;

        //������ ���Ҵ� ���� �ѹ��� �ѹ����� �Ͼ���� ��
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
        //������ ���Ұ� �Ұ��ɻ��½� ����
        if (!canCutableJumpPower)
            return;

        //���� Ű�� ���� ���¸� ����
        if (jumpKeyValue != 0)
            return;

        //�÷��̾ ���� �߿��� ����
        if (body.velocity.y <= 0)
            return;

        //����Ű�� ���� �ð��� �������� �������� ũ�� ����. ���� �ð��� ����� ���ϰ� ����.
        var jumpPowerReduction = Mathf.Lerp(minJumpPower, maxJumpPower, jumpKeyHoldTimeCounter / maxJumpkeyHoldTime);        

        //���� �÷��̾ �����ӵ��� �� ����� ���� ���ҽ�Ŵ.
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * jumpPowerReduction);

        //������ ���� �Ұ����ϰ� ó��.
        canCutableJumpPower = false;        
    }

    void Update()
    {
        jumpKeyHoldTimeCounter += Time.deltaTime;

        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, maxFallSpeed, maxJumpSpeed));
    }
}
