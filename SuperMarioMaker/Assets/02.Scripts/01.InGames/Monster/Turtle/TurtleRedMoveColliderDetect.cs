using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleRedMoveColliderDetect : TurtleMoveColliderDetect
{
    [SerializeField]
    private float groundCheckRayLength;    

    [SerializeField]
    private float groundRayOffset = 0.3f;

    [SerializeField]
    private Rigidbody2D rb;
    
    private void Update()
    {
        if (turtle.State != EnumTurtleState.Move)
            return;

        if (rb.velocity.y != 0f)
            return;        

        if (!Physics2D.Raycast((Vector2)transform.position + rb.velocity.normalized * groundRayOffset, Vector3.down, groundCheckRayLength, platformLayerMask))
        {   
            ChangeMoveDir();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine((Vector2)transform.position + rb.velocity.normalized * groundRayOffset, transform.position + (Vector3)rb.velocity.normalized * groundRayOffset + Vector3.down * groundCheckRayLength);;
    }
}
