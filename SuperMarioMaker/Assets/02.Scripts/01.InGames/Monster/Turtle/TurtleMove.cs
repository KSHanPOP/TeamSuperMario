using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMove : TurtleBase
{
    public override void Press()
    {
        state.Animator.SetTrigger(hashPressed);
        state.EnterState(EnumTurtleState.Idle);
    }
}
