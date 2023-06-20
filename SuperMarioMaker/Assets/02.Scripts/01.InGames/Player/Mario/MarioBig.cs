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

        blinkCoroutine = StartCoroutine(BlinkCoroutine());

        Invoke(nameof(StopInvincible), marioSmall.InvincibleTime);

        playerState.Animator.SetTrigger(hashTransformation);
    }
    public override void EatFireFlower()
    {
        playerState.nextState = marioFire;
        playerState.nextState.Enter();
    }
    protected virtual void StopInvincible()
    {
        playerState.SetNormalLayer();

        StopCoroutine(blinkCoroutine);
        sprite.color = Color.white;
    }
    protected virtual IEnumerator BlinkCoroutine()
    {
        bool isTransparent = true;
        WaitForSeconds changePeriod = new(0.1f);

        while(true)
        {
            isTransparent = !isTransparent;
            sprite.color = isTransparent ? Color.clear : Color.white;
            yield return changePeriod;
        }
    }
}
