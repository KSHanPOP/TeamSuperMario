using UnityEngine;

public class QuestionBlock : Block
{
    private int hashUsed = Animator.StringToHash("Used");
    private Animator animator;
    
    protected override void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (itemType == EnumItems.Blank)
            itemType = EnumItems.Coin;

        base.Awake();
    }

    protected override void CheckItemRemainCount()
    {
        --itemCount;

        if (itemCount == 0)
            animator.SetTrigger(hashUsed);            
    }
}
