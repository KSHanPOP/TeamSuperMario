using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleSpin : DefaultTurtleState
{
    private Transform player;

    public int ScoreCombo { get; set; } = 0;

    [SerializeField]
    private float destroyRangeFromPlayer = 18;

    private void Start()
    {
        player = PlayerState.Instance.transform;
    }

    public override void Enter()
    {
        base.Enter();
        ScoreCombo = 0;
        turtle.State = EnumTurtleState.Spin;
    }
    public override void Press()
    {
        state.Animator.SetTrigger(hashPressed);
        state.EnterState(EnumTurtleState.Idle);
    }

    public override void OnUpdate()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) > destroyRangeFromPlayer)
            Destroy(gameObject);
    }
}
