using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    private enum EnumThwompSate
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

    [SerializeField]
    private float attackCooldown = 0.5f;
    private float lastAttackSequenceTime = 0f;

    private Vector3 moveDir;

    private EnumThwompSate state;
    private EnumThwompSate State
    {
        set
        {
            switch (value)
            {
                case EnumThwompSate.Idle:
                    lastAttackSequenceTime = Time.time;
                    transform.position = startPos;
                    rb2d.velocity = Vector2.zero;
                    break;
                case EnumThwompSate.Attack:
                    animator.SetTrigger(hashAttack);
                    break;
                case EnumThwompSate.Return:
                    animator.SetTrigger(hashReturn);
                    break;
            }
            state = value;
        }
    }

    private void Start()
    {
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
            case EnumThwompSate.Idle:
                UpdateIdle();
                break;
            case EnumThwompSate.Attack:
                UpdateAttack();
                break;
            case EnumThwompSate.Return:
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
        Vector3 adjustPos = (isVertical ? Vector3.left : Vector3.up) * 0.5f;

        var hit1 = Physics2D.Raycast(middlePos + adjustPos, moveDir, maxMoveAbleDisatance, platformLayer);
        var hit2 = Physics2D.Raycast(middlePos - adjustPos, moveDir, maxMoveAbleDisatance, platformLayer);

        if (hit1 && hit2)
            return hit1.distance < hit2.distance ? hit1.distance : hit2.distance;

        if (hit1)
            return hit1.distance;

        if (hit2)
            return hit2.distance;

        return maxMoveAbleDisatance;
    }
    private void UpdateIdle()
    {
        if (Time.time - lastAttackSequenceTime < attackCooldown)
            return;

        float playerDistance =
            isVertical ? player.position.x - middlePos.x : player.position.y - middlePos.y;

        if (Mathf.Abs(playerDistance) < playerDetectionRange)
        {
            State = EnumThwompSate.Attack;
        }
    }
    private void UpdateAttack()
    {
        rb2d.velocity = moveDir * attackVelocity;

        float checkMoveableRange =
            isVertical ? startPos.y - transform.position.y : MathF.Abs(startPos.x - transform.position.x);

        if (checkMoveableRange > platformDistance)
        {
            State = EnumThwompSate.Return;
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
            State = EnumThwompSate.Idle;
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