using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMoveColliderDetect : MoveColliderDetect
{
    [SerializeField]
    Turtle turtle;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
            ChangeMoveDir();

        if(collision.CompareTag("Monster"))
        {
            if (turtle.State == EnumTurtleState.Idle)
                return;

            if(turtle.State == EnumTurtleState.Spin)
            {
                collision.GetComponent<IShakeable>().Shake(transform.position);
                return;
            }

            ChangeMoveDir();
        }
    }
}
