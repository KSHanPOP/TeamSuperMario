using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    protected RuntimeAnimatorController controller;    
    private void Awake()
    {
                       
    }

    public virtual void Enter()
    {

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
