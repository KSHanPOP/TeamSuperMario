using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TurtleMoveColliderDetect : MoveColliderDetect
{
    [SerializeField]
    private Turtle turtle;

    [SerializeField]
    private float spinStateRayOffsetAdjust;

    [SerializeField]
    private float monstDetectRayLength;

    private LayerMask monsterLayerMask;
    private LayerMask platformLayerMask;

    protected override void Awake()
    {
        base.Awake();
        monsterLayerMask = LayerMask.GetMask("Monster");
        platformLayerMask = LayerMask.GetMask("Platform");
    }

    public float RayLength { get { return rayLength; } set { rayLength = value; } }

    protected override void Update()
    {
        if (turtle.State == EnumTurtleState.Idle)
            return;

        if (turtle.State == EnumTurtleState.Move)
        {
            base.Update();
            return;
        }

        if(Physics2D.Raycast((Vector2)transform.position + rayStartOffset, Vector2.up, rayLength, platformLayerMask))
            ChangeMoveDir();

        var hit = Physics2D.Raycast((Vector2)transform.position + rayStartOffset + Vector2.down * spinStateRayOffsetAdjust, Vector2.up, monstDetectRayLength, monsterLayerMask);

        if (hit)
            hit.transform.GetComponent<IShakeable>().Shake(transform.position);
    }
}
