using System.Collections;
using UnityEngine;

public class MarioBig : PlayerBase
{
    protected MarioSmall marioSmall;
    protected MarioFire marioFire;

    private RuntimeAnimatorController fireController;

    protected Coroutine blinkCoroutine;

    protected bool isFire = false;
    protected override void Awake()
    {
        base.Awake();
        marioSmall = GetComponent<MarioSmall>();
        marioFire = GetComponent<MarioFire>();

        fireController = marioFire.Controller;
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

        StartCoroutine(ToSmallTransformationCoroutine());       

        Invoke(nameof(StopInvincible), marioSmall.InvincibleTime);
    }

    public override void EatMushroom()
    {
        Logger.Debug("get score");
    }

    public override void EatFireFlower()
    {
        playerState.nextState = marioFire;
        playerState.nextState.Enter();
        StartCoroutine(BigToFireTransformationCoroutine());
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
    protected void SetTransformationScale(out WaitForSeconds changePeriod, out int count, out float[] scales)
    {
        changePeriod = new(0.1f);

        count = 5;
        scales = new float[count];

        scales[0] = 0.8f;
        scales[1] = 0.9f;
        scales[2] = 0.7f;
        scales[3] = 0.8f;
        scales[4] = 0.6f;
    }
    protected IEnumerator ToSmallTransformationCoroutine()
    {
        SetTransformationScale(out WaitForSeconds changePeriod, out int count, out float[] scales);

        for (int i = 0; i < count; i++)
        {
            spritePivotTransform.localScale = new Vector3(1, scales[i], i);

            if(i == 3)
                blinkCoroutine = StartCoroutine(BlinkCoroutine());

            yield return changePeriod;
        }

        spritePivotTransform.localScale = Vector3.one;
        OnTransformationComplete();
        playerState.nextState.Enter();
        
        yield break;
    }

    protected IEnumerator BigToFireTransformationCoroutine()
    {
        WaitForSeconds changePeriod = new(0.1f);        

        for(int i = 0; i < 5; i++)
        {
            isFire = !isFire;
            playerState.Animator.runtimeAnimatorController = isFire ? fireController : controller;
        }
        
        OnTransformationComplete();
        playerState.nextState.Enter();

        yield break;
    }
}
