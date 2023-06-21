using UnityEngine;

public class MoveColliderDetect : MonoBehaviour
{
    private ObjectMove move;
    private Collider2D collisionTrigger;

    private void Awake()
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
        collisionTrigger.offset = -collisionTrigger.offset;
        move.ChangeMoveDir();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") ||
            collision.CompareTag("Monster"))
            
        {            
            ChangeMoveDir();
        }
    }
}
