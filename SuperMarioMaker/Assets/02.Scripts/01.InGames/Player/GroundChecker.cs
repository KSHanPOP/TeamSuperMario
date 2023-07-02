using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    private float groundDetectLength = 0.95f;
    [SerializeField]
    private Vector3 colliderOffset;

    [SerializeField]
    private float colliderOffsetY;

    [SerializeField]
    private LayerMask platformLayer;

    private bool isGround = true;    

    private void Update()
    {
        var startPos = transform.position + Vector3.down * colliderOffsetY;

        isGround = 
            Physics2D.Raycast(startPos + colliderOffset, Vector2.down, groundDetectLength, platformLayer) || 
            Physics2D.Raycast(startPos - colliderOffset, Vector2.down, groundDetectLength, platformLayer);
    }

    private void OnDrawGizmos()
    {   
        if (isGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + Vector3.down * colliderOffsetY + colliderOffset, transform.position + Vector3.down * colliderOffsetY + colliderOffset + Vector3.down * groundDetectLength);
        Gizmos.DrawLine(transform.position + Vector3.down * colliderOffsetY - colliderOffset, transform.position + Vector3.down * colliderOffsetY - colliderOffset + Vector3.down * groundDetectLength);
    }

    public bool IsGround() => isGround;
}
