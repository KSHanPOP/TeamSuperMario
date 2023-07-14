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
    private bool isPressingKey;

    private bool isTryRun;

    [SerializeField]
    private float runSpeedMultiplier = 2f;

    [SerializeField]
    private float rapidAcceleraton = 2f;

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
        isTryRun = context.ReadValue<float>() == 1;
    }

    private void Update()
    {
        directionX = limiter.CharacterCanMove ? moveKeyValue : 0f;

        isPressingKey = directionX != 0;

        desiredVelocity = new Vector2(directionX, 0f) * maxSpeed;

        if (isTryRun)
            desiredVelocity *= runSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();

        Move();
    }

    private void Move()
    {
        velocity = body.velocity;

        acceleration = isGround ? maxAcceleration : maxAirAcceleration;
        deceleration = isGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = isGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (isPressingKey)
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
}
