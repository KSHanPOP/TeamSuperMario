using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonsterBase
{
    public EnumTurtleState State { get; set; } = EnumTurtleState.Move;

    private float originHeight;

    [SerializeField]
    private float smallHeight;

    [SerializeField]
    private ObjectMove move;

    [SerializeField]
    private TurtleMoveColliderDetect turtleMoveColliderDetect;

    private BoxCollider2D turtleCollider;
    private BoxCollider2D turtleTrigger;

    private Vector2 originColliderSize;
    private Vector2 originColliderOffset;

    private Vector2 originTriggerSize;
    private Vector2 originTriggerOffset;

    [SerializeField]
    private Vector2 smallColliderSize;
    [SerializeField]
    private Vector2 smallColliderOffset;
    [SerializeField]
    private Vector2 smallTriggerSize;
    [SerializeField]
    private Vector2 smallTriggerOffset;


    protected override void Awake()
    {
        base.Awake();
        Setcolliders();
        originHeight = monsterHeight;
    }

    private void Setcolliders()
    {
        var colliders = GetComponents<BoxCollider2D>();

        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
                turtleTrigger = collider;
            else
                turtleCollider = collider;
        }

        originColliderSize = turtleCollider.size;
        originColliderOffset = turtleCollider.offset;

        originTriggerSize = turtleTrigger.size;
        originTriggerOffset = turtleTrigger.offset;
    }

    public void SetOriginSize()
    {
        turtleCollider.size = originColliderSize;
        turtleCollider.offset = originColliderOffset;

        turtleTrigger.size = originTriggerSize;
        turtleTrigger.offset = 
            new Vector2(move.GetDir() < 0 ? originColliderOffset.x : - originColliderOffset.x, originColliderOffset.y);

        monsterHeight = originHeight;
    }

    public void SetSmallSize()
    {
        turtleCollider.size = smallColliderSize;
        turtleCollider.offset = smallColliderOffset;

        turtleTrigger.size = smallTriggerSize;

        turtleTrigger.offset = 
            new Vector2(move.GetDir() < 0 ? smallTriggerOffset.x : - smallTriggerOffset.x, smallTriggerOffset.y);        

        monsterHeight = smallHeight;
    }

    public void ChangeHeight(float newHeight)
    {
        monsterHeight = newHeight;
    }

    public void SetOriginHeight()
    {
        monsterHeight = originHeight;
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
