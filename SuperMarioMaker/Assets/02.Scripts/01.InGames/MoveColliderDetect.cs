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

    protected float rayDir = -1;

    protected bool dirChanged = false;

    protected virtual void Awake()
    {
        layerMask = LayerMask.GetMask("Platform");
    }

    protected virtual void FixedUpdate()
    {
        dirChanged = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision();
    }

    protected virtual void CheckCollision()
    {
        rayStartPos = (Vector2)transform.position + rayOffset * Vector2.up;        

        if (Physics2D.Raycast(rayStartPos + Vector2.up * rayInterval, Vector2.right * rayDir, rayLength, layerMask) ||
            Physics2D.Raycast(rayStartPos + Vector2.down * rayInterval, Vector2.right * rayDir, rayLength, layerMask))
        {   
            ChangeMoveDir();
        }
    }

    public void ChangeMoveDir()
    {
        if (dirChanged)
            return;

        dirChanged = true;
        rayDir = -rayDir;
        move.ReverseMoveDir();
    }

    private void OnDrawGizmos()
    {
        var startPos = (Vector2)transform.position + Vector2.up * rayOffset;

        Gizmos.DrawLine(startPos + Vector2.up * rayInterval, startPos + Vector2.up * rayInterval + Vector2.right * rayDir * rayLength);
        Gizmos.DrawLine(startPos + Vector2.down * rayInterval, startPos + Vector2.down * rayInterval + Vector2.right * rayDir * rayLength);
    }
}
