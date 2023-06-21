
public class QuestionBlock : Block
{
    protected override void Awake()
    {
        if (itemType == EnumItems.None)
            itemType = EnumItems.Coin;

        base.Awake();
    }
}
