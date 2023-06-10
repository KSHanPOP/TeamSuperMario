using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected PlayerState playerState;

    [SerializeField]
    protected RuntimeAnimatorController controller;    
    protected virtual void Awake()
    {        
        playerState = GetComponent<PlayerState>();                       
    }

    public virtual void Enter()
    {
        playerState.CurrState = this;
        playerState.Animator.runtimeAnimatorController = controller;
    }
    public virtual void EatMushroom()
    {

    }
    public virtual void EatFireFlower()
    {

    }
    public virtual void Hit()
    {

    }
    public virtual void PlayerUpdate()
    {

    }
    public virtual void PlayerOnTriggerEnter()
    {

    }

    public virtual void PlayerOnTriggerStay()
    {

    }

    public virtual void PlayerOnTriggerExit()
    {

    }
}
