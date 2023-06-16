using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected Vector2 dir = Vector2.left;
    public Vector2 Dir { get { return dir; } set { dir = value; } }

    protected Rigidbody2D rb;    
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        DoMove();
    }

    private void Update()
    {
        DoMove();
    }

    public virtual void DoMove()
    {
        rb.velocity = speed * dir;
    }

    public virtual void ChangeMoveDir()
    {
        dir = - dir;        
    }

    public virtual void Stop()
    {
        dir = Vector2.zero;
    }
}

