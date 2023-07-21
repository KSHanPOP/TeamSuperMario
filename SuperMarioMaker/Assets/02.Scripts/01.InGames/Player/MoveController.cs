using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    private Rigidbody2D body;
    GroundChecker ground;

    [SerializeField]
    public float maxSpeed = 10f;
    [SerializeField]
    public float maxAcceleration = 52f;
    [SerializeField]
    public float maxDecceleration = 52f;
    [SerializeField]
    public float maxTurnSpeed = 80f;
    [SerializeField]
    public float maxAirAcceleration;
    [SerializeField]
    public float maxAirDeceleration;
    [SerializeField]
    public float maxAirTurnSpeed = 80f;

    public float directionX;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    private MovementLimmiter limiter;
    private float moveKeyValue;

    private bool isGround;
    private bool isPressingMoveKey = false;

    private bool isTryRun = false;
    private bool isTrySit = false;
    private bool isSitting = false;

    [SerializeField]
    private float runMaxSpeedMultiplier = 2f;

    [SerializeField]
    private float runAccelMultiplier = 1.5f;

    [SerializeField]
    private float rapidAcceleraton = 2f;

    [SerializeField]
    private PlayerState playerState;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundChecker>();
    }

    private void Start()
    {
        limiter = MovementLimmiter.instance;
    }

    public void TryMove(InputAction.CallbackContext context)
    {
        moveKeyValue = context.ReadValue<float>();        
    }

    public void TryRun(InputAction.CallbackContext context)
    {
        isTryRun = context.ReadValue<float>() != 0;
    }

    public void TrySit(InputAction.CallbackContext context)
    {
        isTrySit = context.ReadValue<float>() != 0;        
    }

    private void Update()
    {
        directionX = limiter.CharacterCanMove ? moveKeyValue : 0f;
        isPressingMoveKey = moveKeyValue != 0 && limiter.CharacterCanMove;

        CheckSitting();        

        desiredVelocity = new Vector2(directionX, 0f) * maxSpeed;

        if (isTryRun)
            desiredVelocity *= runMaxSpeedMultiplier;

        isGround = ground.IsGround();

        Move();
    }

    //private void FixedUpdate()
    //{
    //    isGround = ground.IsGround();

    //    Move();
    //}

    private void CheckSitting()
    {
        if (isTrySit && ground.IsGround())
            StartSitting();

        if (!isTrySit)
            EndSitting();

        if (isSitting)
        {
            directionX = 0;
            isPressingMoveKey = false;
            return;
        }
    }

    public void StartSitting()
    {
        if (!limiter.CharacterCanMove)
            return;

        isSitting = true;
        playerState.Sit = true;
        playerState.CurrState.StartSit();
    }

    public void EndSitting()
    {
        if (!limiter.CharacterCanMove)
            return;

        isSitting = false;
        playerState.Sit = false;
        playerState.CurrState.EndSit();
    }

    public bool GetSit()
    {   
        return isSitting;
    }

    private void Move()
    {
        velocity = body.velocity;

        acceleration = isGround ? maxAcceleration : maxAirAcceleration;
        deceleration = isGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = isGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (isTryRun)
            acceleration *= runAccelMultiplier;

        if (isPressingMoveKey)
        {
            maxSpeedChange = directionX * velocity.x < 0 ?
                turnSpeed : acceleration * Mathf.Lerp(rapidAcceleraton, 1f, velocity.x / desiredVelocity.x);
        }
        else
        {
            maxSpeedChange = deceleration;
        }        

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange * Time.deltaTime);

        body.velocity = velocity;
    }

    public float GetMaxSpeed() => maxSpeed * runMaxSpeedMultiplier;
}
