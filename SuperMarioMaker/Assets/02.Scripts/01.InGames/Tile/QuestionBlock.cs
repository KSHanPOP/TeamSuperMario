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
        switch (itemType)
        {
            case EnumItems.Coin:
                SoundManager.Instance.PlaySFX("Coin");
                break;
            case EnumItems.Mushroom:
                SoundManager.Instance.PlaySFX("Item");
                break;
            case EnumItems.FireFlower:
                SoundManager.Instance.PlaySFX("Item");
                break;
        }

        --itemCount;

        if (itemCount == 0)
            animator.SetTrigger(hashUsed);            
    }
}
