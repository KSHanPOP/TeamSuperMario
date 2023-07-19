using System;
using UnityEngine;

public class Thwomp : MonoBehaviour, IAwake
{
    private enum EnumThwompState
    {
        None = -1,
        Idle,
        Attack,
        Return,
    }
    private enum EnumThwompDir
    {
        None = -1,
        Down,
        Left,
        Right,
    }

    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashReturn = Animator.StringToHash("Return");

    private Transform player;
    private Vector3 startPos;
    private Vector3 middlePos;
    private float platformDistance;

    [SerializeField]
    private EnumThwompDir dir;

    private bool isVertical;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private RuntimeAnimatorController sideAnimationController;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float verticalPlayerDetectionRange;

    [SerializeField]
    private float horizontalPlayerDetectionRange;

    private float playerDetectionRange;

    [SerializeField]
    private float attackVelocity;

    [SerializeField]
    private float returnVelocity;

    [SerializeField]
    private float maxMoveAbleDisatance = 20f;

    [SerializeField]
    private LayerMask platformLayer;

    private LayerMask layerMask;

    [SerializeField]
    private float attackCooldown = 0.5f;
    private float lastAttackSequenceTime = 0f;

    private Vector3 moveDir;

    private EnumThwompState state;
    private EnumThwompState State
    {
        set
        {
            switch (value)
            {
                case EnumThwompState.Idle:
                    lastAttackSequenceTime = Time.time;
                    transform.position = startPos;
                    rb2d.velocity = Vector2.zero;
                    break;
                case EnumThwompState.Attack:
                    animator.SetTrigger(hashAttack);
                    break;
                case EnumThwompState.Return:
                    animator.SetTrigger(hashReturn);
                    break;
            }
            state = value;
        }
    }

    public void OnAwake()
    {
        Logger.Debug("thwomp awake");
        enabled = true;
    }

    private void Start()
    {
        layerMask = LayerMask.GetMask("Platform", "MonsterNoCollision");

        startPos = transform.position;
        middlePos = transform.position + (Vector3.right + Vector3.down) * 0.5f;
        isVertical = dir == EnumThwompDir.Down;
        playerDetectionRange = isVertical ? verticalPlayerDetectionRange : horizontalPlayerDetectionRange;
        SetMoveDir();
        platformDistance = GetPlatformDistance() - (isVertical ? 1f : 0.75f);

        if (!isVertical)
        {
            animator.runtimeAnimatorController = sideAnimationController;
            if (dir == EnumThwompDir.Right)
                spriteRenderer.flipX = true;
        }

        player = PlayerState.Instance.transform;
    }

    private void Update()
    {
        switch (state)
        {
            case EnumThwompState.Idle:
                UpdateIdle();
                break;
            case EnumThwompState.Attack:
                UpdateAttack();
                break;
            case EnumThwompState.Return:
                UpdateReturn();
                break;
        }
    }
    private void SetMoveDir()
    {
        moveDir = dir switch
        {
            EnumThwompDir.Down => Vector3.down,
            EnumThwompDir.Left => Vector3.left,
            EnumThwompDir.Right => Vector3.right,
            _ => Vector3.zero
        };
    }
    private float GetPlatformDistance()
    {
        var collider = GetComponent<BoxCollider2D>();

        collider.enabled = false;

        Vector3 adjustPos = (isVertical ? Vector3.left : Vector3.up) * 0.5f;

        var hit1 = Physics2D.Raycast(middlePos + adjustPos, moveDir, maxMoveAbleDisatance, layerMask);
        var hit2 = Physics2D.Raycast(middlePos - adjustPos, moveDir, maxMoveAbleDisatance, layerMask);

        collider.enabled = true;

        if (hit1 && hit2)
            return hit1.distance < hit2.distance ? hit1.distance : hit2.distance;

        if (hit1)
            return hit1.distance;

        if (hit2)
            return hit2.distance;

        return maxMoveAbleDisatance;
    }

    private bool IsPlayerInRange()
    {
        float playerDistance = 0f;

        switch (dir)
        {
            case EnumThwompDir.Down:
                {
                    if (player.position.y > middlePos.y)
                        return false;

                    playerDistance = player.position.x - middlePos.x;
                }
                break;
            case EnumThwompDir.Left:
                {
                    if (player.position.x > middlePos.x)
                        return false;

                    playerDistance = player.position.y - middlePos.y;
                }
                break;                
            case EnumThwompDir.Right:
                {
                    if(player.position.x < middlePos.x) 
                        return false;

                    playerDistance = player.position.y - middlePos.y;
                }
                break;
        }  

        return Mathf.Abs(playerDistance) < playerDetectionRange;
    }

    private void UpdateIdle()
    {
        if (Time.time - lastAttackSequenceTime < attackCooldown)
            return;

        float playerDistance =
            isVertical ? player.position.x - middlePos.x : player.position.y - middlePos.y;

        if (IsPlayerInRange())
        {
            State = EnumThwompState.Attack;
        }
    }
    private void UpdateAttack()
    {
        rb2d.velocity = moveDir * attackVelocity;

        float checkMoveableRange =
            isVertical ? startPos.y - transform.position.y : MathF.Abs(startPos.x - transform.position.x);

        if (checkMoveableRange > platformDistance)
        {
            SoundManager.Instance.PlaySFX("Thwomp");
            State = EnumThwompState.Return;
        }
    }
    private void UpdateReturn()
    {
        rb2d.velocity = -moveDir * returnVelocity;

        float checkReturn =
            isVertical ? startPos.y - transform.position.y :
            dir == EnumThwompDir.Left ? startPos.x - transform.position.x : transform.position.x - startPos.x;

        if (checkReturn < 0f)
        {
            State = EnumThwompState.Idle;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerAnimation>().Hit();
        }
    }

    public int GetDir() => (int)dir;
    public void SetDir(int idx) => dir = (EnumThwompDir)idx;
}
