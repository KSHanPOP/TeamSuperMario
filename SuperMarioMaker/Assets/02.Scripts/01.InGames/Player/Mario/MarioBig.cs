using UnityEngine;

public class MarioBig : PlayerBase
{
    private MarioSmall marioSmall;
    private MarioFire marioFire;
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
        playerState.nextState = marioSmall;        
    }
}
