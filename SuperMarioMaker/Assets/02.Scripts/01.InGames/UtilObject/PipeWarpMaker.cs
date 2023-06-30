using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeWarpMaker : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask fallingLayer;

    private int playerLayerIntValue;
    private int fallingLayerIntValue;


    [SerializeField]
    private float rayLength;

    [SerializeField]
    private bool isTopSide;

    [SerializeField]
    private float warpMoveSpeed;

    [SerializeField]
    private float warpTime;

    private Vector2 rayOrigin;

    private float rightKeyValue;

    private float downKeyValue;

    private bool inWarpSequence = false;

    private void Awake()
    {
        playerLayerIntValue = (int)Mathf.Log(playerLayer.value, 2);
        fallingLayerIntValue = (int)Mathf.Log(fallingLayer.value, 2);

        rayOrigin.x = isTopSide ? transform.position.x + (1 - rayLength) * 0.5f : transform.position.x - 0.6f;
        rayOrigin.y = isTopSide ? transform.position.y + 0.6f : transform.position.y + (-1 + rayLength) * 0.5f;
    }

    public void  GetRightKeyValue(InputAction.CallbackContext context)
    {
        rightKeyValue = context.ReadValue<float>();
    }

    public void GetDownKeyValue(InputAction.CallbackContext context)
    {
        downKeyValue = context.ReadValue<float>();
    }

    private void Update()
    {
        if (inWarpSequence)
            return;

        var hit = Physics2D.Raycast(rayOrigin, isTopSide ? Vector2.right : Vector2.down, rayLength, playerLayer);        

        if (!hit)
            return;

        if ((isTopSide ? downKeyValue : rightKeyValue) != 1f)
            return;        

        var player = hit.transform;        

        if (!player.GetComponent<GroundChecker>().IsGround())
            return;

        StartCoroutine(PipeWarpCoroutine(player));        
    }

    IEnumerator PipeWarpCoroutine(Transform player)
    {
        inWarpSequence = true;

        MovementLimmiter.instance.CharacterCanMove = false;
        player.gameObject.layer = fallingLayerIntValue;
        player.GetComponent<PlayerLayerSortController>().SetLayerSort(EnumSpriteLayerOrder.InPipe);

        float startTime = Time.time;
        var playerRb = player.GetComponent<Rigidbody2D>();
        float playerOriginGravScale = playerRb.gravityScale;

        Vector2 newVelocity =
            (isTopSide ? Vector2.down : Vector2.right) * warpMoveSpeed;

        playerRb.gravityScale = 0f;

        while (Time.time - startTime < warpTime)
        {
            playerRb.velocity = newVelocity;   
            yield return null;
        }

        inWarpSequence = false;

        playerRb.velocity = Vector2.zero;
        playerRb.gravityScale = playerOriginGravScale; 
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayOrigin, (isTopSide ? Vector2.right : Vector2.down) * rayLength);
    }
}

    


