using System.Collections;
using UnityEngine;
public class MarioFire : MarioBig
{
    private MarioBig marioBig;

    protected RuntimeAnimatorController bigController;

    protected override void Awake()
    {
        base.Awake();
        marioBig = GetComponent<MarioBig>();

        bigController = marioBig.Controller;        
    }
    public override void Hit()
    {
        StartTransformation();
        playerState.nextState = marioBig;
        playerState.nextState.Enter();
        isFire = true;
        StartCoroutine(BigToFireTransformationCoroutine());

        Invoke(nameof(StopInvincible), marioSmall.InvincibleTime);
    }
    public override void EatMushroom()
    {
        Logger.Debug("get score");
    }
    public override void EatFireFlower()
    {
        Logger.Debug("get score");
    }

    protected override void ChangeControllder()
    {
        playerState.Animator.runtimeAnimatorController = isFire ? controller : bigController;
    }

    protected override void ActionAfterChange()
    {
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }  
}
