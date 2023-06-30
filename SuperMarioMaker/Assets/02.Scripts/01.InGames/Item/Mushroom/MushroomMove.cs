using UnityEngine;

public class MushroomMove : ObjectMove, IShakeable
{
    [SerializeField]
    private float upForceWhenCollisonShakingBlock;
    protected override void InitVelocity()
    {
        dir = -dir;
        velocityX = dir * speed;
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
