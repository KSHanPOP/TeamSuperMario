using System.Collections;
using UnityEngine;

public class MarioBig : PlayerBase
{
    protected MarioSmall marioSmall;
    protected MarioFire marioFire;

    protected Coroutine blinkCoroutine;
    protected override void Awake()
    {
        base.Awake();
        marioSmall = GetComponent<MarioSmall>();
        marioFire = GetComponent<MarioFire>();
    }
    public override void Enter()
    {
        base.Enter();
        SetBigCollider();
    }
    public override void Hit()
    {
        StartTransformation();
        playerState.nextState = marioSmall;

        blinkCoroutine = StartCoroutine(CoBlinkCoroutine());

        Invoke(nameof(StopInvincible), marioSmall.InvincibleTime);
    }
    protected virtual void StopInvincible()
    {
        playerState.SetNormalLayer();

        StopCoroutine(blinkCoroutine);
        sprite.color = Color.white;
    }
    protected virtual IEnumerator CoBlinkCoroutine()
    {
        bool isBlink = true;

        //float changePeriod = marioSmall.InvincibleTime / (marioSmall.blinkCount *2 + 1);
        //WaitForSeconds waitTime = new WaitForSeconds(changePeriod);
        //Logger.Debug(changePeriod);

        while(true)
        {
            isBlink = !isBlink;
            sprite.color = isBlink ? Color.clear : Color.white;
            yield return null;
        }
    }
}
