using UnityEngine;

public class QuestionBlock : Block
{
    private int hashUsed = Animator.StringToHash("Used");
    private Animator animator;
    
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponentInChildren<Animator>();
    }

    public override void InitSetting()
    {
        if (itemCount == 0)
            itemCount = 1;

        if (itemType == EnumItems.Blank)
            itemType = EnumItems.Coin;

        SetStartSprite();
    }
    protected override void CheckItemRemainCount()
    {
        --itemCount;

        if (itemCount == 0)
            animator.SetTrigger(hashUsed);            
    }
}
