using UnityEngine;

[RequireComponent(typeof(MarioSmall), typeof(MarioBig), typeof(MarioFire))]
public class PlayerState : MonoBehaviour
{
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
}


