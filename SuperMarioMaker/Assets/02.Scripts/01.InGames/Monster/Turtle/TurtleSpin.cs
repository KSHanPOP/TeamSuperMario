using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleSpin : TurtleBase
{
    public override void Enter()
    {
        state.EnterState(EnumTurtleState.Idle);
    }
}
