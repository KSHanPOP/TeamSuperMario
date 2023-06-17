using UnityEngine;

public class MarioSmall : PlayerBase
{
    protected MarioBig marioBig;
    protected MarioFire marioFire;

    [SerializeField]
    private float invincibleTime = 1f;
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
