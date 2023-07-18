using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMoveColliderDetect : MonsterMoveColliderDetect
{
    [SerializeField]
    protected Turtle turtle;

    protected LayerMask originLayerMask;
    protected LayerMask platformLayerMask;

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
                SoundManager.Instance.PlaySFX("Kick");

                target.GetComponent<IShakeable>().Shake(transform.position);
            }
        }
    }
}
