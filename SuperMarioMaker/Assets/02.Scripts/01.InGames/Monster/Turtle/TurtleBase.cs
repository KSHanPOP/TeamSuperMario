using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurtleBase : MonoBehaviour
{
    protected readonly int hashPressed = Animator.StringToHash("Pressed");
    protected readonly int hashIdleTimeDone = Animator.StringToHash("IdleTimeDone");

    protected TurtleState state;

    protected Turtle turtle;

    [SerializeField]
    protected float moveSpeed;
    protected virtual void Awake()
    {
        state = GetComponent<TurtleState>();
        turtle = GetComponent<Turtle>();
    }

    public virtual void Press()
    {
        
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void Enter()
    {
        state.CurrState = this;
        state.objectMove.ChageSpeed(moveSpeed);
    }
    public virtual void Exit()
    {

    }
}
