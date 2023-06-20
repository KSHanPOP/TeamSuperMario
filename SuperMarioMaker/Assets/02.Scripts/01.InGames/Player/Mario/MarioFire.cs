using UnityEngine;
public class MarioFire : MarioBig
{   
    private MarioBig marioBig;
    protected override void Awake()
    {
        base.Awake();        
        marioBig = GetComponent<MarioBig>();
    }

    public override void EatFireFlower()
    {
        Logger.Debug("get score");
    }
}
