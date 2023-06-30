using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonsterBase
{
    public EnumTurtleState State { get; set; } = EnumTurtleState.Move;

    [SerializeField]
    private float turtleHeight;

    [SerializeField]
    private ObjectMove move;

    [SerializeField]
    private TurtleMoveColliderDetect turtleMoveColliderDetect;

    protected override void Awake()
    {
        monsterHeight = turtleHeight;       
    }
    public override bool IsAttackable(Vector2 pos, float minDistanceToPress)
    {
        if (State == EnumTurtleState.Idle)
        {
            if ((pos.x - transform.position.x) * move.GetDir() > 0)
                turtleMoveColliderDetect.ChangeMoveDir();

            return true;
        }       

        return pos.y > monsterHeight * minDistanceToPress + transform.position.y;
    }
}
