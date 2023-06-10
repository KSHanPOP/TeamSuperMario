using UnityEngine;

public class MarioSmall : PlayerBase
{
    MarioBig marioBig;
    MarioFire marioFire;
    protected override void Awake()
    {
        base.Awake();
        marioBig = GetComponent<MarioBig>();
        marioFire = GetComponent<MarioFire>();
    }
    public override void EatMushroom()
    {
        playerState.nextState = marioBig;
        playerState.Animator.transform.localPosition = Vector3.up * 0.5f;
    }
}
