using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMove : TurtleBase
{
    public override void Enter()
    {
        base.Enter();
        turtle.State = EnumTurtleState.Move;
        turtle.SetOriginSize();
    }

    public override void Press()
    {
        state.Animator.SetTrigger(hashPressed);
        state.EnterState(EnumTurtleState.Idle);
    }
}
