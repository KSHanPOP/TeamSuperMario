using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TurtleMoveColliderDetect : MonsterMoveColliderDetect
{
    [SerializeField]
    private Turtle turtle;

    private LayerMask originLayerMask;
    private LayerMask platformLayerMask;

    protected override void Awake()
    {
        base.Awake();
        originLayerMask = layerMask;
        platformLayerMask = LayerMask.GetMask("Platform");
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (turtle.State == EnumTurtleState.Idle)
            return;

        if (turtle.State == EnumTurtleState.Move)
        {
            layerMask = originLayerMask;
            base.CheckCollisionIgnoreSelf();
        }        

        if (turtle.State == EnumTurtleState.Spin)
        {
            layerMask = platformLayerMask;
            base.CheckCollision();

            var target = collision.collider;

            if (target.CompareTag("Monster"))
            {
                target.GetComponent<IShakeable>().Shake(transform.position);
            }
        }
    }
}
