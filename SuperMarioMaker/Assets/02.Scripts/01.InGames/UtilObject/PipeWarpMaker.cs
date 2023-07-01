using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeWarpMaker : MonoBehaviour
{
    private static bool inWarpSequence = false;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask fallingLayer;

    private int playerLayerIntValue;
    private int fallingLayerIntValue;

    [SerializeField]
    private float rayLength;

    private bool isVertical;

    [SerializeField]
    private Pipe pipe;

    private EnumPipeEntrancePos pipeEntrancePos;

    [SerializeField]
    private float warpMoveSpeed;

    [SerializeField]
    private float warpTime;

    private Vector2 rayOrigin;

    private float axisKeyValue;

    private float downKeyValue;

    private float jumpKeyValue;

    private Coroutine pipeWarpCoroutine;

    private void Awake()
    {
        playerLayerIntValue = (int)Mathf.Log(playerLayer.value, 2);
        fallingLayerIntValue = (int)Mathf.Log(fallingLayer.value, 2);

        pipeEntrancePos = pipe.PipeEntrancePos;
        isVertical = (int)pipeEntrancePos % 2 == 0;        
    }

    private void Start()
    {
        SetRayOrigin();
    }

    private void SetRayOrigin()
    {
        rayOrigin.x = pipeEntrancePos switch
        {
            EnumPipeEntrancePos.Top => transform.position.x + (1 - rayLength) * 0.5f,
            EnumPipeEntrancePos.Left => transform.position.x - 0.6f,
            EnumPipeEntrancePos.Bottom => transform.position.x + (1 - rayLength) * 0.5f,
            EnumPipeEntrancePos.Right => transform.position.x + 0.6f,
            _ => 0f,
        };        

        rayOrigin.y = pipeEntrancePos switch
        {
            EnumPipeEntrancePos.Top => transform.position.y + 0.6f,
            EnumPipeEntrancePos.Left => transform.position.y + (-1 + rayLength) * 0.5f,
            EnumPipeEntrancePos.Bottom => transform.position.y - 0.6f,
            EnumPipeEntrancePos.Right => transform.position.y + (-1 + rayLength) * 0.5f,
            _ => 0f,
        };        
    }

    public void GetAxisKeyValue(InputAction.CallbackContext context)
    {
        axisKeyValue = context.ReadValue<float>();
    }
    public void GetDownKeyValue(InputAction.CallbackContext context)
    {
        downKeyValue = context.ReadValue<float>();
    }
    public void GetJumpKeyValue(InputAction.CallbackContext context)
    {
        jumpKeyValue = context.ReadValue<float>();
    }

    private void Update()
    {
        if (inWarpSequence)
            return;

        var hit = Physics2D.Raycast(rayOrigin, isVertical ? Vector2.right : Vector2.down, rayLength, playerLayer);        

        if (!hit)
            return;

        Logger.Debug("player hit");

        if (!CheckKeyValue())
            return;        

        var player = hit.transform;

        if (!CheckGroundValue(player))
            return;        

        pipeWarpCoroutine = StartCoroutine(PipeWarpCoroutine(player));
    }

    private bool CheckKeyValue()
    {
        switch (pipeEntrancePos)
        {
            case EnumPipeEntrancePos.Top:                
                return downKeyValue == 1f;
            case EnumPipeEntrancePos.Left:
                return axisKeyValue == 1f;
            case EnumPipeEntrancePos.Bottom:
                return jumpKeyValue == 1f;
            case EnumPipeEntrancePos.Right:
                return axisKeyValue == -1f;
        }

        return false;
    }

    private bool CheckGroundValue(Transform player)
    {
        var value = player.GetComponent<GroundChecker>().IsGround();

        return pipeEntrancePos == EnumPipeEntrancePos.Bottom ? true : value;
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

        Vector2 newVelocity = WarpVelocity();            

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

    private Vector2 WarpVelocity() => pipeEntrancePos switch
    {
        EnumPipeEntrancePos.Top => Vector2.down,
        EnumPipeEntrancePos.Left => Vector2.right,
        EnumPipeEntrancePos.Bottom => Vector2.up,
        EnumPipeEntrancePos.Right => Vector2.left,
        _=> Vector2.zero,
    };

    private void OnDestroy()
    {
        if (pipeWarpCoroutine != null)
        {
            inWarpSequence = false;
            StopCoroutine(pipeWarpCoroutine);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayOrigin, (isVertical ? Vector2.right : Vector2.down) * rayLength);
    }
}
