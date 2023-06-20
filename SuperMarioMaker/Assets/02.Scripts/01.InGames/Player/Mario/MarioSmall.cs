using UnityEngine;

public class MarioSmall : PlayerBase
{
    private int hashDie = Animator.StringToHash("Die");

    protected MarioBig marioBig;
    protected MarioFire marioFire;

    [SerializeField]
    private float invincibleTime = 2.6f;
    public float InvincibleTime 
    { 
        get { return invincibleTime; }         
    }
    protected override void Awake()
    {
        base.Awake();
        marioBig = GetComponent<MarioBig>();
        marioFire = GetComponent<MarioFire>();
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
        spriteTransform.localPosition = Vector3.up;

        playerState.Animator.SetTrigger(hashTransformation);
    }

    public override void EatFireFlower()
    {
        StartTransformation();
        playerState.nextState = marioFire;
        spriteTransform.localPosition = Vector3.up;

        playerState.Animator.SetTrigger(hashTransformation);
    }

    public override void Hit()
    {
        playerState.SetFallingLayer();

        playerState.Animator.SetTrigger(hashDie);
    }
    public override void OnTransformationComplete()
    {
        base.OnTransformationComplete();        
        playerState.SetNormalLayer();
    }
}
