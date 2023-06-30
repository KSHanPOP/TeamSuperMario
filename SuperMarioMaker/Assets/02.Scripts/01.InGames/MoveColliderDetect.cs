using UnityEngine;

public class MoveColliderDetect : MonoBehaviour
{
    [SerializeField]
    protected ObjectMove move;    

    [SerializeField]
    protected float rayInterval;

    [SerializeField]
    protected float rayOffset;

    [SerializeField]
    protected float rayLength;

    protected Vector2 rayStartPos;

    protected LayerMask layerMask;

    protected float dir = -1;

    protected virtual void Awake()
    {
        layerMask = LayerMask.GetMask("Platform", "Monster");
    }

    protected virtual void Update()
    {
        rayStartPos = (Vector2)transform.position + Vector2.up * rayOffset;

        if (Physics2D.Raycast(rayStartPos + Vector2.up * rayInterval, Vector2.right * dir, rayLength, layerMask) ||
            Physics2D.Raycast(rayStartPos + Vector2.down * rayInterval, Vector2.right * dir, rayLength, layerMask))
            ChangeMoveDir();
    }

    public void ChangeMoveDir()
    {
        dir = -dir;
        move.ReverseMoveDir();
    }

    private void OnDrawGizmos()
    {
        var startPos = (Vector2)transform.position + Vector2.up * rayOffset;

        Gizmos.DrawLine(startPos + Vector2.up * rayInterval, startPos + Vector2.up * rayInterval + Vector2.right * dir * rayLength);
        Gizmos.DrawLine(startPos + Vector2.down * rayInterval, startPos + Vector2.down * rayInterval + Vector2.right * dir * rayLength);
    }
}
