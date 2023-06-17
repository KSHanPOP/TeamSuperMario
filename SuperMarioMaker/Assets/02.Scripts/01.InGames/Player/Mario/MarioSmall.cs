using UnityEngine;

public class MarioSmall : PlayerBase
{
    protected MarioBig marioBig;
    protected MarioFire marioFire;

    [SerializeField]
    private float invincibleTime = 2.6f;
    public float InvincibleTime 
    { 
        get { return invincibleTime; }         
    }

    public readonly int blinkCount = 12;

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
        spriteTransform.localPosition = Vector3.zero;        
    }
    public override void EatMushroom()
    {
        StartTransformation();
        playerState.nextState = marioBig;
        spriteTransform.localPosition = Vector3.up * 0.5f;        
    }
    public override void Hit()
    {
        StartTransformation();        
    }
    public override void OnTransformationComplete()
    {
        base.OnTransformationComplete();        
        playerState.SetNormalLayer();
    }
}
