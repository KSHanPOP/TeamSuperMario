using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeWarpConnector : MonoBehaviour, ICommandStackAble
{
    public static bool IsLinking = false;
    public static LinkedList<PipeWarpConnector> Connectors = new();
    public static LinkedList<PipeWarpConnector> Buffer = new();
    public static PipeWarpConnector Sender;
    public static bool IsPlaying = false;

    private bool isLinked = false;

    [SerializeField]
    private PipeWarpController controller;

    [SerializeField]
    private SpriteRenderer highlight;

    [SerializeField]
    private Color highlighColor;

    [SerializeField]
    private SpriteRenderer arrowhead;

    [SerializeField]
    private BoxCollider2D boxCollider2D;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private int lineVerticleCount;

    private int lineLastIndex;

    [SerializeField]
    private float minControlPointMultiplier;

    [SerializeField]
    private float maxControlPointMultiplier;

    [SerializeField, Range(0f,1f)]
    private float distanceThreshholdController;

    public EnumWarpConnectorState State;  

    private Vector3 startPos;

    private Vector3[] linePoses;    

    private Vector3 targetPos;

    private Camera cam;
    
    private PipeWarpConnector receviedPipe;
    private void Awake()
    {
        Connectors.AddLast(this);
        lineRenderer.positionCount = lineVerticleCount;
        lineLastIndex = lineVerticleCount - 1;
        linePoses = new Vector3[lineVerticleCount];
    }
    private void Start()
    {
        cam = Camera.main;
        startPos = transform.position + (controller.IsVertical ? Vector3.right : Vector3.down) * 0.5f;        
    }
    public static void StartTest()
    {
        IsPlaying = true;

        IsLinking = false;

        foreach (var connector in Connectors)
        {
            if (connector != null)
            {
                connector.ClearAll();
                Buffer.AddLast(connector);
            }
        }
    }
    public static void StopTest()
    {
        IsPlaying = false;

        SwapBuffer();

        foreach (var connector in Connectors)
        {
            connector.DrawLinkedLine();
        }
    }

    public void StartLink()
    {
        if (IsPlaying)
            return;

        if (IsLinking)
            return;

        IsLinking = true;
        isLinked = false;
        controller.DisconnectWarpPoint();

        foreach (var connector in Connectors)
        {
            if (connector != null)
            {
                connector.DrawHighlight();
                connector.State = EnumWarpConnectorState.Receiver;
                Buffer.AddLast(connector);
            }
        }

        EnableLine();

        State = EnumWarpConnectorState.Sender;
        Sender = this;
        highlight.enabled = false;
    }

    public void StopLinking()
    {
        if (!IsLinking)
            return;

        SwapBuffer();

        foreach (var connector in Connectors)
        {
            connector.ClearDrawsExceptLinked();
        }

        IsLinking = false;
    }

    private static void SwapBuffer()
    {
        Connectors.Clear();

        (Connectors, Buffer) = (Buffer, Connectors);
    }

    public void DrawHighlight()
    {
        highlight.color = highlighColor;
        highlight.enabled = true;

        highlight.transform.localScale = boxCollider2D.size;
        highlight.transform.localPosition = boxCollider2D.offset;
    }

    public void ClearHighlight()
    {
        highlight.enabled = false;
    }

    private void DrawLinkedLine()
    {
        if (isLinked)
        {
            EnableLine();  
        }
    }

    public void ClearAll()
    {
        DisableLine();
        highlight.enabled = false;
    }

    public void ClearDrawsExceptLinked()
    {
        if (!isLinked)
        {
            DisableLine();
        }

        highlight.enabled = false;        
    }
    private Vector3 GetMousePos()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -cam.transform.position.z;

        return cam.ScreenToWorldPoint(mousePosition);
    }
    private void CompleteLinking()
    {
        if (!IsLinking)
            return;        

        if(State == EnumWarpConnectorState.Receiver)
        {
            Sender.SetDest(controller, startPos);
            receviedPipe = Sender;
        }
    }
    public void SetDest(PipeWarpController dest, Vector3 destPos)
    {
        isLinked = true;
        controller.SetDestWarpPoint(dest);

        bool isRight = destPos.x - startPos.x > 0f;

        targetPos = destPos + (isRight ? Vector3.up : Vector3.down) * 0.5f;

        StopLinking();
        UpdateLine();        
    }
    public void OnMouseDown()
    {
        CompleteLinking();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopLinking();
        }

        if (IsLinking)
            UpdateLine();
    }

    public void UpdateLine()
    {
        if (State != EnumWarpConnectorState.Sender)
            return;

        Vector3 dest = IsLinking ? GetMousePos() : targetPos;

        float t = 1 / (float)(lineLastIndex);
        for (int i = 0; i < lineVerticleCount; i++)
        {
            linePoses[i] = CalculateBezierPoint(startPos, GetControlPoint(startPos, dest), dest, t * i);
        }

        lineRenderer.SetPositions(linePoses);

        arrowhead.transform.position = dest;
        float angle = CalculateAngle(linePoses[(int)(lineVerticleCount * 0.5f)], dest);
        arrowhead.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;
        return point;
    }
    private float CalculateAngle(Vector3 point1, Vector3 point2)
    {
        Vector3 direction = point2 - point1;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    public Vector2 GetControlPoint(Vector2 A, Vector2 B)
    {        
        Vector2 midPoint = (A + B) / 2f;
        
        Vector2 AB = B - A;
        
        Vector2 normal = new Vector2(-AB.y, AB.x);
        
        float distance = Vector2.Distance(A, B);

        float scaleFactor = Mathf.Lerp(maxControlPointMultiplier, minControlPointMultiplier, distance * distanceThreshholdController);
        
        Vector2 C = midPoint + scaleFactor * normal;

        return C;
    }

    private void EnableLine()
    {
        lineRenderer.enabled = true;
        arrowhead.enabled = true;
    }

    private void DisableLine()
    {
        lineRenderer.enabled = false;
        arrowhead.enabled = false;
    }
    public void DisConnect()
    {
        isLinked = false;
        DisableLine();
        controller.DisconnectWarpPoint();
    }

    public void ConnectAgain()
    {
        isLinked = true;
        EnableLine();
        controller.ConnectWarpPoint();
    }

    private void DisConnectWhenDisable()
    {
        if (receviedPipe == null)
            return;

        if (receviedPipe.controller.GetDestWarpPoint() == controller)
        {
            receviedPipe.DisConnect();
        }
    }

    private void OnDestroy()
    {
        DisConnectWhenDisable();
    }
    public void DisableCommand()
    {
        DisConnectWhenDisable();
    }
    public void EnableCommand()
    {
        if (receviedPipe == null)
            return;

        if (receviedPipe.controller.GetDestWarpPoint() == controller)
        {
            receviedPipe.ConnectAgain();
        }
    }
}

public enum EnumWarpConnectorState
{
    None = -1,
    Sender,
    Receiver,
}
