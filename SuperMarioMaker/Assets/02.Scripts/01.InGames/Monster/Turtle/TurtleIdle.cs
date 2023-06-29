using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleIdle : TurtleBase
{
    public override void Enter()
    {
        base.Enter();
        turtle.State = EnumTurtleState.Idle;
        turtle.SetSmallSize();
    }

    public override void Press()
    {
        state.Animator.SetTrigger(hashPressed);
        state.EnterState(EnumTurtleState.Spin);
    }
}
