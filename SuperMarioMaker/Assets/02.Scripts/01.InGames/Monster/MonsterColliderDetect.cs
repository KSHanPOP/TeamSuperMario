using UnityEngine;

public class MonsterColliderDetect : MonoBehaviour
{
    private MonsterMove move;
    private Collider2D collisionTrigger;

    private void Awake()
    {
        move = GetComponent<MonsterMove>(); 
        var colilders = GetComponents<Collider2D>();
        foreach (var collider in colilders )
        {
            if(collider.isTrigger)
                collisionTrigger = collider;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {   
            collisionTrigger.offset = - collisionTrigger.offset;
            move.ChangeMoveDir();
        }
    }
}
