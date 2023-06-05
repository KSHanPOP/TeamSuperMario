using UnityEngine;

public class MonsterColliderDetect : MonoBehaviour
{
    //[SerializeField]
    //protected float searchLength = 0.51f;

    //[SerializeField]
    //Rigidbody2D rb;
    //Vector2 dir = Vector2.left;    

    //RaycastHit2D[] results = new RaycastHit2D[1];

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

    //private void Update()
    //{
    //    if (rb.Cast(dir, results, searchLength) == 0)
    //        return;

    //    if (!results[0].collider.CompareTag("Player"))
    //    {
    //        var result = results[0];

    //        dir = - dir;
    //        move.ChangeMoveDir();
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(rb.position, rb.position + searchLength * dir);
    //}
}
