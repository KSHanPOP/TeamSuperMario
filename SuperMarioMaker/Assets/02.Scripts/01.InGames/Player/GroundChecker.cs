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
    private LayerMask platformLayer;

    private bool isGround;

    private void Update()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        isGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundDetectLength, platformLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundDetectLength, platformLayer);
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (isGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundDetectLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundDetectLength);
    }

    public bool IsGround() => isGround;
}