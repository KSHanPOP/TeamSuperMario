using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMove : TurtleBase
{
    public override void Press()
    {
        state.EnterState(EnumTurtleState.Idle);
    }
}
