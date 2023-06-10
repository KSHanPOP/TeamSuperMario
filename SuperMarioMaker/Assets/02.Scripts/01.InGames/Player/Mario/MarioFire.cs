using UnityEngine;
public class MarioFire : PlayerBase
{
    MarioSmall marioSmall;
    MarioBig marioBig;
    protected override void Awake()
    {
        base.Awake();
        marioSmall = GetComponent<MarioSmall>();
        marioBig = GetComponent<MarioBig>();
    }
    public override void Hit()
    {
        marioBig.Hit();
    }
}
