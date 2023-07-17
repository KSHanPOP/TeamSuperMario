using UnityEngine;
using System.Collections;

public class MarioSmall : MarioDefaultState
{
    private int hashDie = Animator.StringToHash("Die");

    protected MarioBig marioBig;
    protected MarioFire marioFire;

    private RuntimeAnimatorController bigController;
    private RuntimeAnimatorController fireController;

    [SerializeField]
    private float invincibleTime = 2.6f;
    public float InvincibleTime 
    { 
        get { return invincibleTime; }         
    }

    [SerializeField]
    private float dieJumpForce = 5f;

    protected override void Awake()
    {
        base.Awake();
        marioBig = GetComponent<MarioBig>();
        marioFire = GetComponent<MarioFire>();

        bigController = marioBig.Controller;
        fireController = marioFire.Controller;
    }
    public override void Enter()
    {
        base.Enter();
        SetSmallCollider();
        spriteTransform.localPosition = Vector3.up * 0.5f;        
    }
    public override void EatMushroom()
    {
        StartTransformation();
        playerState.nextState = marioBig;
        playerState.nextState.Enter();
        spriteTransform.localPosition = Vector3.up;

        StartCoroutine(SmallToBigTransformationCoroutine());
    }
    

    public override void EatFireFlower()
    {
        StartTransformation();
        playerState.nextState = marioFire;
        playerState.nextState.Enter();
        spriteTransform.localPosition = Vector3.up;

        StartCoroutine(SmallToFireTransformationCoroutine());
    }

    public override void Hit()
    {
        playerState.Animator.SetTrigger(hashDie);

        Die();
    }

    public override void Die()
    {
        MovementLimmiter.instance.CharacterCanMove = false;

        isTransformingSequence = true;

        playerState.IsAttckable = false;

        rb.velocity = Vector2.zero;        

        rb.AddForce(Vector2.up * dieJumpForce, ForceMode2D.Impulse);        

        playerState.SetFallingLayer();


        //////
        Invoke(nameof(ResetGame), 5f);
        //////
    }

    public override void DoJump()
    {
        SoundManager.Instance.PlaySFX("Jump");
    }

    public void ResetGame()
    {
        TileManager.Instance.Restart();
    }

    public override void OnTransformationComplete()
    {
        base.OnTransformationComplete();        
        playerState.SetNormalLayer();
    }   

    private void SetTransformationScale(out WaitForSeconds changePeriod, out int count, out float[] scales)
    {
        changePeriod = new(0.1f);

        count = 5;
        scales = new float[count];

        scales[0] = 0.8f;
        scales[1] = 0.7f;
        scales[2] = 1.0f;
        scales[3] = 0.8f;
        scales[4] = 1.0f;
    }
    IEnumerator SmallToBigTransformationCoroutine()
    {
        SetTransformationScale(out WaitForSeconds changePeriod, out int count, out float[] scales);

        for (int i = 0; i < count; i++)
        {            
            spritePivotTransform.localScale = new Vector3(1, scales[i], i);
            yield return changePeriod;
        }

        OnTransformationComplete();
        yield break;
    }

    IEnumerator SmallToFireTransformationCoroutine()
    {
        SetTransformationScale(out WaitForSeconds changePeriod, out int count, out float[] scales);

        bool isFire = false;

        for (int i = 0; i < count; i++)
        {
            isFire = !isFire;

            playerState.Animator.runtimeAnimatorController = isFire ? fireController : bigController;

            spritePivotTransform.localScale = new Vector3(1, scales[i], i);

            yield return changePeriod;
        }

        playerState.Animator.runtimeAnimatorController = fireController;

        OnTransformationComplete();
        yield break;
    }
}
