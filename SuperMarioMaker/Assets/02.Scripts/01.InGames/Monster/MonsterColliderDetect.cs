using UnityEngine;

public class MonsterColliderDetect : MonoBehaviour
{
    [SerializeField]
    protected float searchLength = 0.51f;

    [SerializeField]
    Rigidbody2D rb;
    Vector2 dir = Vector2.left;

    private MonsterMove move;

    RaycastHit2D[] results = new RaycastHit2D[1];

    private void Awake()
    {
        move = GetComponent<MonsterMove>(); 
    }

    private void Update()
    {
        if (rb.Cast(dir, results, searchLength) > 0)
        {   
            if (!results[0].collider.CompareTag("Player"))
            {
                Logger.Debug("hit");
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.position, rb.position + Vector2.left);
    }
}
