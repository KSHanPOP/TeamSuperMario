using UnityEngine;

public class MonsterColliderDetect : MonoBehaviour
{
    protected bool onCollisionEnter;
    protected bool OnCollisionEnter { get { return onCollisionEnter; } }
    
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    protected float layLength;

    Rigidbody2D rb;
    Vector2 dir = Vector2.left;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }    
    private void Update()
    {
        RaycastHit2D[] results = null;
        

    }

    private void OnDrawGizmos()
    {
        if(onCollisionEnter)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir * layLength);
    }
}
