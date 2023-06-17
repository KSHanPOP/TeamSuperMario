using UnityEngine;

[RequireComponent(typeof(MarioSmall), typeof(MarioBig), typeof(MarioFire))]
public class PlayerState : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    int normalLayer;

    [SerializeField]
    int invincibleLayer;

    [SerializeField]
    int fallingLayer;

    public PlayerBase CurrState { get; set; }
    public PlayerBase nextState { get; set; }
    public Animator Animator { get; set; }
    
    private MarioSmall marioSmall;
    
    private MarioBig marioBig;
    
    private MarioFire marioFire;
    public void Awake()
    {   
        marioSmall = GetComponent<MarioSmall>();
        marioBig = GetComponent<MarioBig>();
        marioFire = GetComponent<MarioFire>();
    }

    public void SetStartState(MarioState state)
    {
        switch (state)
        {
            case MarioState.Small:
                marioSmall.Enter();
                break;
            case MarioState.Big:
                marioBig.Enter();
                break;
            case MarioState.Fire:
                marioFire.Enter();
                break;
        }
    }
    public void SetNormalLayer()
    {
        player.layer = normalLayer;        
    }

    public void SetInvincibleLayer()
    {
        player.layer = invincibleLayer;        
    }

    public void SetFallingLayer()
    {
        player.layer = fallingLayer;
    }
}


