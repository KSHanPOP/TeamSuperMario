using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TurtleMoveColliderDetect : MoveColliderDetect
{
    [SerializeField]
    private Turtle turtle;
    
    private LayerMask platformLayerMask;

    protected override void Awake()
    {
        base.Awake();        
        platformLayerMask = LayerMask.GetMask("Platform");
    }    

    protected override void Update()
    {
        if (turtle.State == EnumTurtleState.Idle)
            return;

        if(turtle.State == EnumTurtleState.Move)
            base.Update();

        if(turtle.State == EnumTurtleState.Spin)
        {
            rayStartPos = (Vector2)transform.position + Vector2.up * rayOffset;

            if (Physics2D.Raycast(rayStartPos + Vector2.up * rayInterval, Vector2.right * dir, rayLength, platformLayerMask) ||
                Physics2D.Raycast(rayStartPos + Vector2.down * rayInterval, Vector2.right * dir, rayLength, platformLayerMask))
                ChangeMoveDir();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (turtle.State != EnumTurtleState.Spin)
            return;

        var target = collision.collider;

        if(target.CompareTag("Monster"))
        {
            target.GetComponent<IShakeable>().Shake(transform.position);
        }        
    }
}
