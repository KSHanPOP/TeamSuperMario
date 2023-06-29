using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleIdle : TurtleBase
{
    private Coroutine idleCoroutine;    

    [SerializeField]
    private float[] idleTime;

    public override void Enter()
    {
        base.Enter();
        turtle.State = EnumTurtleState.Idle;
        turtle.SetSmallSize();

        idleCoroutine = StartCoroutine(IdleCoroutine());
    }

    public override void Press()
    {
        state.Animator.SetTrigger(hashPressed);
        state.EnterState(EnumTurtleState.Spin);

        if (idleCoroutine != null)
            StopCoroutine(idleCoroutine);
    }
    IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(idleTime[0]);

        state.Animator.SetTrigger(hashIdleTimeDone);

        yield return new WaitForSeconds(idleTime[1]);

        state.Animator.SetTrigger(hashIdleTimeDone);
        state.EnterState(EnumTurtleState.Move);
    }
}
