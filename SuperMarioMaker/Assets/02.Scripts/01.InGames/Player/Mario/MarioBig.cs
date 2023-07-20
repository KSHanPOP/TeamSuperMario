using System.Collections;
using UnityEngine;

public class MarioBig : DefaultMario
{
    protected MarioSmall marioSmall;
    protected MarioFire marioFire;

    protected RuntimeAnimatorController targetController;

    protected Coroutine blinkCoroutine;
    
    protected bool isFire = false;
    protected override void Awake()
    {
        base.Awake();
        marioSmall = GetComponent<MarioSmall>();
        marioFire = GetComponent<MarioFire>();

        targetController = marioFire.Controller;
    }
    public override void Enter()
    {
        base.Enter();
        SetBigCollider();
    }
    public override void Hit()
    {
        SoundManager.Instance.PlaySFX("Warp");

        StartTransformation();
        playerState.nextState = marioSmall;

        StartCoroutine(ToSmallTransformationCoroutine());       

        Invoke(nameof(StopInvincible), marioSmall.InvincibleTime);
    }

    public override void EatFireFlower()
    {
        if (isTransformingSequence)
            return;

        base.EatFireFlower();

        SoundManager.Instance.PlaySFX("Powerup");

        StartTransformation();
        playerState.nextState = marioFire;
        isFire = false;
        StartCoroutine(BigToFireTransformationCoroutine());
    }

    public override void StartSit()
    {
        SetSmallCollider();
    }

    public override void EndSit()
    {
        SetBigCollider();

        //Vector3 offset = Vector3.right * 0.375f;
        //LayerMask platformLayer = LayerMask.GetMask("Platform");
        //RaycastHit2D hit1 = Physics2D.Raycast(transform.position - offset, Vector2.up, bigBlockDetectLength, platformLayer);
        //RaycastHit2D hit2 = Physics2D.Raycast(transform.position + offset, Vector2.up, bigBlockDetectLength, platformLayer);
    }

    public override void Die()
    {
        playerState.nextState = marioSmall;
        playerState.nextState.Enter();
        playerState.CurrState.Die();
    }
    public override void PlayJumpSound()
    {
        SoundManager.Instance.PlaySFX("BigJump");        
    }

    protected virtual void StopInvincible()
    {
        playerState.SetNormalLayer();

        StopCoroutine(blinkCoroutine);
        sprite.color = Color.white;
    }
    protected virtual IEnumerator BlinkCoroutine()
    {
        bool isTransparent = false;
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
        WaitForSeconds changePeriod = new(0.1f);

        int count = 5;
        float[]  scales = new float[count];

        scales[0] = 0.8f;
        scales[1] = 0.9f;
        scales[2] = 0.7f;
        scales[3] = 0.8f;
        scales[4] = 0.6f;

        for (int i = 0; i < count; i++)
        {
            spritePivotTransform.localScale = new Vector3(1, scales[i], i);

            if (i == 3)
                blinkCoroutine = StartCoroutine(BlinkCoroutine());

            yield return changePeriod;
        }

        spritePivotTransform.localScale = Vector3.one;
        OnTransformationComplete();
        playerState.nextState.Enter();
        
        yield break;
    }

    protected virtual IEnumerator BigToFireTransformationCoroutine()
    {
        WaitForSeconds changePeriod = new(0.1f);        

        for(int i = 0; i < 5; i++)
        {
            isFire = !isFire;
            ChangeControllder();
            yield return changePeriod;
        }
        
        OnTransformationComplete();
        ActionAfterChange();
        playerState.nextState.Enter();        
    }
    protected virtual void ChangeControllder()
    {
        playerState.Animator.runtimeAnimatorController = isFire ? targetController : controller;
    }    
    protected virtual void ActionAfterChange()
    {
        playerState.SetNormalLayer();
    }
}
