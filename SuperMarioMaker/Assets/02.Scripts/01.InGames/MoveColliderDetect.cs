using UnityEngine;

public class MoveColliderDetect : MonoBehaviour
{
    [SerializeField]
    protected ObjectMove move;    

    [SerializeField]
    protected Vector2 rayStartOffset;
    [SerializeField]
    protected float rayLength;

    protected LayerMask layerMask;

    protected virtual void Awake()
    {
        layerMask = LayerMask.GetMask("Platform", "Monster");
    }

    protected virtual void Update()
    {
        if (Physics2D.Raycast((Vector2)transform.position + rayStartOffset, Vector2.up, rayLength, layerMask))
            ChangeMoveDir();
    }

    public void ChangeMoveDir()
    {
        rayStartOffset.x = - rayStartOffset.x;
        move.ReverseMoveDir();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine((Vector2)transform.position + rayStartOffset, (Vector2)transform.position + rayStartOffset + Vector2.up * rayLength);
    }
}
