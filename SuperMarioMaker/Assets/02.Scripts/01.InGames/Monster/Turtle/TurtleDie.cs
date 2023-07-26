using UnityEngine;

public class TurtleDie : AttackedShaked
{
    [SerializeField]
    private Turtle turtle;

    private readonly int hashDie = Animator.StringToHash("Die");

    protected override void SetAnimation()
    {
        animator.SetTrigger(hashDie);
    }

    public override void Shake(Vector2 attackerPos)
    {
        if (turtle.State == EnumTurtleState.Spin &&
            attackerPos.y < transform.position.y)
        {
            return;
        }

        base.Shake(attackerPos);
    }
}
