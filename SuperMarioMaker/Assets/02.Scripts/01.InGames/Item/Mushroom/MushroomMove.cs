using UnityEngine;

public class MushroomMove : ObjectMove
{
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
}
