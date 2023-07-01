using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterMoveColliderDetect : MoveColliderDetect
{
    [SerializeField]
    protected BoxCollider2D boxCollder;
    protected override void Awake()
    {
        layerMask = LayerMask.GetMask("Platform", "Monster");
    }  
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollisionIgnoreSelf();
    }

    protected virtual void CheckCollisionIgnoreSelf()
    {
        rayStartPos = (Vector2)transform.position + rayOffset * Vector2.up;

        if (RaycastAllResult(Vector2.up) ||
            RaycastAllResult(Vector2.down))
        {
            ChangeMoveDir();
        }
    }

    protected virtual bool RaycastAllResult(Vector2 verticalPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPos + verticalPos * rayInterval, Vector2.right * rayDir, rayLength, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                return true; 
            }
        }
        return false;
    } 
}
