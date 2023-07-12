using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeWarpController : MonoBehaviour
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

    public bool IsVertical { get { return isVertical; } }

    [SerializeField]
    private Pipe pipe;

    private EnumPipeEntrancePos pipeEntrancePos;

    public bool IsSmallState;

    [SerializeField]
    private float smallStateWarpSpeed;

    [SerializeField]
    private float bigStateWarpSpeed;

    [SerializeField]
    private float enterWarpTime;
    
    private float smallStateExtiWarpTime;
    
    private float bigStateExtiWarpTime;

    [SerializeField]
    private bool isConnected = false;

    [SerializeField]
    private PipeWarpController destWarpPoint;

    private Vector2 rayOrigin;

    private float axisKeyValue;

    private float downKeyValue;

    private float jumpKeyValue;

    public Transform Player;

    private Coroutine pipeWarpCoroutine;    

    private void Awake()
    {
        playerLayerIntValue = (int)Mathf.Log(playerLayer.value, 2);
        fallingLayerIntValue = (int)Mathf.Log(fallingLayer.value, 2);

        pipeEntrancePos = pipe.PipeEntrancePos;
        isVertical = (int)pipeEntrancePos % 2 == 0;
        SetWarpTimer();
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

    private void SetWarpTimer()
    {
        smallStateExtiWarpTime = (1f / smallStateWarpSpeed) + (isVertical ? -0.05f : 0.15f);        

        bigStateExtiWarpTime = isVertical ? (2f / bigStateWarpSpeed) - 0.05f : smallStateExtiWarpTime;
    }

    private void Update()
    {
        if (!isConnected)
            return;

        if (inWarpSequence)
            return;

        var hit = Physics2D.Raycast(rayOrigin, isVertical ? Vector2.right : Vector2.down, rayLength, playerLayer);

        if (!hit)
            return;

        if (!CheckKeyValue())
            return;

        Player = hit.transform;        

        if (!CheckCanWarp())
            return;

        pipeWarpCoroutine = StartCoroutine(EnterWarpCoroutine());
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

    private bool CheckCanWarp()
    {
        var value = Player.GetComponent<GroundChecker>().IsGround();

        return pipeEntrancePos == EnumPipeEntrancePos.Bottom ? true : value;
    }

    private void StopPlayerControllable()
    {
        MovementLimmiter.instance.CharacterCanMove = false;
        Player.gameObject.layer = fallingLayerIntValue;
        Player.GetComponent<PlayerLayerSortController>().SetLayerSort(EnumSpriteLayerOrder.InPipe);
    }

    private void StartPlayerControllable()
    {
        MovementLimmiter.instance.CharacterCanMove = true;
        Player.gameObject.layer = playerLayerIntValue;
        Player.GetComponent<PlayerLayerSortController>().SetLayerSort(EnumSpriteLayerOrder.Player);
    }

    IEnumerator EnterWarpCoroutine()
    {
        inWarpSequence = true;

        IsSmallState = PlayerState.Instance.IsSmallState();
        destWarpPoint.Player = Player;
        destWarpPoint.IsSmallState= IsSmallState;

        float startTime = Time.time;
        Vector2 newVelocity = WarpVelocity();

        var playerRb = Player.GetComponent<Rigidbody2D>();
        float playerOriginGravScale = playerRb.gravityScale;

        StopPlayerControllable();

        playerRb.gravityScale = 0f;

        while (Time.time - startTime < enterWarpTime)
        {
            if (playerRb == null)
                StopSequence();

            playerRb.velocity = newVelocity;
            yield return null;
        }

        destWarpPoint.StartExitWarpCoroutine(playerRb, playerOriginGravScale);
        yield break;        
    }

    public void StartExitWarpCoroutine(Rigidbody2D playerRb, float playerOriginGravScale)
    {
        pipeWarpCoroutine = StartCoroutine(ExitWarpCoroutine(playerRb, playerOriginGravScale));
    }

    IEnumerator ExitWarpCoroutine(Rigidbody2D playerRb, float playerOriginGravScale)
    {        
        MovePlayerPosToExitEntrance();

        float startTime = Time.time;
        Vector2 newVelocity = WarpVelocity() * -1;
        
        var timer = IsSmallState ? smallStateExtiWarpTime : bigStateExtiWarpTime;

        while (Time.time - startTime < timer)
        {
            if (playerRb == null)
                StopSequence();

            playerRb.velocity = newVelocity;
            yield return null;
        }

        if (pipeEntrancePos == EnumPipeEntrancePos.Top)
            playerRb.velocity = Vector2.zero;
        
        playerRb.gravityScale = playerOriginGravScale;

        StartPlayerControllable();

        yield return new WaitForSeconds(0.2f);

        inWarpSequence = false;

        yield break;
    }

    private void MovePlayerPosToExitEntrance()
    {
        pipe.SleepPlant();

        Vector3 destPos = isVertical ?
            transform.position + Vector3.right * 0.5f : transform.position + Vector3.down * 1.01f;        

        Player.position = pipeEntrancePos switch
        {
            EnumPipeEntrancePos.Top => destPos + (IsSmallState ? Vector3.zero : Vector3.down),
            EnumPipeEntrancePos.Left => destPos,
            EnumPipeEntrancePos.Bottom => destPos,
            EnumPipeEntrancePos.Right => destPos,
            _ => Vector2.zero
        };        
    }

    private Vector2 WarpVelocity()
    {
        float speedMultiflier = IsSmallState ? smallStateWarpSpeed : bigStateWarpSpeed;

        return speedMultiflier * pipeEntrancePos switch
        {
            EnumPipeEntrancePos.Top => Vector2.down,
            EnumPipeEntrancePos.Left => Vector2.right,
            EnumPipeEntrancePos.Bottom => Vector2.up,
            EnumPipeEntrancePos.Right => Vector2.left,
            _ => Vector2.zero,
        }; 
    }
    private void StopSequence()
    {
        if (pipeWarpCoroutine != null)
        {
            inWarpSequence = false;
            StopCoroutine(pipeWarpCoroutine);
        }
    }

    public void SetDestWarpPoint(PipeWarpController dest)
    {
        destWarpPoint = dest;
        isConnected = true;
    }

    public PipeWarpController GetDestWarpPoint()
    {
        return destWarpPoint;
    }
    public void DisconnectWarpPoint()
    {
        isConnected = false;
    }
    public void ConnectWarpPoint()
    {
        isConnected = true;
    }

    private void OnDestroy()
    {
        StopSequence();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayOrigin, (isVertical ? Vector2.right : Vector2.down) * rayLength);
    }
}
