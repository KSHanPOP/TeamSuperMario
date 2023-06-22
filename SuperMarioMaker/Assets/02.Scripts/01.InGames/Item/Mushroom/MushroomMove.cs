using UnityEngine;

public class MushroomMove : ObjectMove, IShakeable
{
    [SerializeField]
    private float upForceWhenCollisonShakingBlock;

    private SpriteRenderer spriteRenderer;    

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
        dir = -dir;
    }

    public override void ChangeMoveDir()
    {   
        base.ChangeMoveDir();
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void Shake(Vector2 blockPos)
    {
        Logger.Debug("shake!");

        if((transform.position.x - blockPos.x) * rb.velocity.x < 0)
            GetComponent<MoveColliderDetect>().ChangeMoveDir();

        rb.velocity = Vector2.right * rb.velocity.x;
        rb.AddForce(Vector2.up * upForceWhenCollisonShakingBlock, ForceMode2D.Impulse);
    }
}
