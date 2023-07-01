using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MarioSmall), typeof(MarioBig), typeof(MarioFire))]
public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }

    [SerializeField]
    private GameObject player;

    [SerializeField]
    public BlockDetector blockDetector;

    [SerializeField]
    int normalLayer;

    [SerializeField]
    int invincibleLayer;

    [SerializeField]
    int fallingLayer;

    public BoxCollider2D playerCollider;

    public BoxCollider2D playerTrigger;

    private bool isAttackable = true;
    public bool IsAttckable { set { isAttackable = value; } get { return isAttackable; } }

    [SerializeField]
    private float actionBuffer = 0.1f;
    private float lastActionBufferInputTime;

    [SerializeField]
    private float actionCooltime = 0.5f;
    private float lastActionTime;


    public PlayerBase CurrState { get; set; }
    public PlayerBase nextState { get; set; }
    public Animator Animator { get; set; }

    private MarioSmall marioSmall;

    private MarioBig marioBig;

    private MarioFire marioFire;
    public void Awake()
    {
        Instance = this;

        marioSmall = GetComponent<MarioSmall>();
        marioBig = GetComponent<MarioBig>();
        marioFire = GetComponent<MarioFire>();

        lastActionBufferInputTime = -actionBuffer;
        lastActionTime = -actionCooltime;
    }

    public void SetStartState(MarioState state)
    {
        switch (state)
        {
            case MarioState.Small:
                marioSmall.Enter();
                break;
            case MarioState.Big:
                marioBig.Enter();
                break;
            case MarioState.Fire:
                marioFire.Enter();
                break;
        }
    }
    public void SetNormalLayer()
    {
        player.layer = normalLayer;
    }

    public void SetInvincibleLayer()
    {
        player.layer = invincibleLayer;
    }

    public void SetFallingLayer()
    {
        player.layer = fallingLayer;
    }

    public void TryAction(InputAction.CallbackContext context)
    {        
        if (context.ReadValue<float>() == 1)
        {
            lastActionBufferInputTime = Time.time;
        }
    }

    private void Update()
    {
        if((Time.time - lastActionBufferInputTime) < actionBuffer &&
            Time.time - lastActionTime > actionCooltime)
        {
            CurrState.DoAction();
            lastActionBufferInputTime = -actionBuffer;
            lastActionTime = Time.time;
        }
    }

    public bool IsSmallState()
    {
        return CurrState == marioSmall;
    }
}


