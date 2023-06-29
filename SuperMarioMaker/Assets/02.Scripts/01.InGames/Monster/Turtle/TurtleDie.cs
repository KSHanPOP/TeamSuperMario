using UnityEngine;

public class TurtleDie : AttackedShaked
{
    private readonly int hashDie = Animator.StringToHash("Die");

    protected override void SetAnimation()
    {
        animator.SetTrigger(hashDie);
    }
}
