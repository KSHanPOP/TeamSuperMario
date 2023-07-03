using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleRedMoveColliderDetect : TurtleMoveColliderDetect
{
    [SerializeField]
    private float groundCheckRayLength;    

    [SerializeField]
    private float groundRayOffset;
    private void Update()
    {
        if (turtle.State != EnumTurtleState.Move)
            return;
        
        if (!Physics2D.Raycast(transform.position + Vector3.right * groundRayOffset, Vector3.down, groundCheckRayLength, platformLayerMask))
        {
            groundRayOffset = -groundRayOffset;
            ChangeMoveDir();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.right * groundRayOffset, transform.position + Vector3.right * groundRayOffset + Vector3.down * groundCheckRayLength);
    }
}
