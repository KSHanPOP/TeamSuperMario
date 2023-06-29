using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeWarpMaker : MonoBehaviour
{
    private int playerLayer;

    [SerializeField]
    private float rayLength;

    [SerializeField]
    private bool isTopSide;

    private Vector2 rayOrigin;

    private float rightKeyValue;

    private float downKeyValue;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        rayOrigin.x = isTopSide ? transform.position.x + (1 - rayLength) * 0.5f : transform.position.x - 0.6f;
        rayOrigin.y = isTopSide ? transform.position.y + 0.6f : transform.position.y + (-1 + rayLength) * 0.5f;

        var hit = Physics2D.Raycast(rayOrigin, isTopSide ? Vector2.right : Vector2.down, rayLength, playerLayer);

        if (!hit)
            return;

        if ((isTopSide ? downKeyValue : rightKeyValue) != 1f)
            return;

        //워프 애니메이션 실행
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayOrigin, (isTopSide ? Vector2.right : Vector2.down) * rayLength);
    }
}

    


