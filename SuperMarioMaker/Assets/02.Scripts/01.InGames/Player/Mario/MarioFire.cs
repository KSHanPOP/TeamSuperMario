using System.Collections;
using UnityEngine;
public class MarioFire : MarioBig
{
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashFired = Animator.StringToHash("Fired");

    private MarioBig marioBig;

    protected RuntimeAnimatorController bigController;    

    [SerializeField]
    FireBall fireball;

    private MovementLimmiter limmiter;

    protected override void Awake()
    {
        base.Awake();
        marioBig = GetComponent<MarioBig>();

        bigController = marioBig.Controller;        
    }
    public void Start()
    {
        limmiter = MovementLimmiter.instance;
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
    public override void DoAction()
    {
        if (!limmiter.CharacterCanMove)
            return;

        playerState.Animator.SetTrigger(hashFire);   
    }

    public void Fired()
    {
        bool isLeft = sprite.flipX;

        Vector3 fireInstancePos = new Vector2(isLeft ? -0.375f : 0.375f, 0.25f);

        var instanced = Instantiate<FireBall>(fireball, transform.position + fireInstancePos, Quaternion.identity, DynamicTileManager.Instance.DynamicObjHolder);

        instanced.FireWithDirection(isLeft);

        playerState.Animator.SetTrigger(hashFired);
    }
}
