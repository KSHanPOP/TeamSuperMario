using UnityEngine;

public class MoveColliderDetect : MonoBehaviour
{
    private ObjectMove move;
    private Collider2D collisionTrigger;

    protected virtual void Awake()
    {
        move = GetComponent<ObjectMove>();
        var colilders = GetComponents<Collider2D>();
        foreach (var collider in colilders)
        {
            if (collider.isTrigger)
                collisionTrigger = collider;
        }
    }

    public void ChangeMoveDir()
    {
        collisionTrigger.offset = new Vector2(-collisionTrigger.offset.x, collisionTrigger.offset.y);
        move.ReverseMoveDir();
    }  

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") ||
            collision.CompareTag("Monster"))
            
        {            
            ChangeMoveDir();
        }
    }
}
