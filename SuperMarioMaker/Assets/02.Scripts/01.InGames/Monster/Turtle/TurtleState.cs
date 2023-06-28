using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleState : MonoBehaviour, IPressable
{
    private TurtleBase currState;
    public TurtleBase CurrState { get { return currState; } set { currState = value; } }

    [SerializeField]
    private Animator animator;
    public Animator Animator { get { return animator; } }

    [SerializeField]
    TurtleIdle idle;

    [SerializeField]
    TurtleMove move;

    [SerializeField]
    TurtleSpin spin;

    [SerializeField]
    public ObjectMove objectMove;

    private void Awake()
    {
        currState = move;
    }

    public void Press()
    {
        currState.Press();
    }
    public void Update()
    {
        currState.OnUpdate();
    }

    public void EnterState(EnumTurtleState state)
    {
        switch (state)
        {
            case EnumTurtleState.Move:
                move.Enter();
                break;
            case EnumTurtleState.Idle:
                idle.Enter();
                break;
            case EnumTurtleState.Spin:
                spin.Enter();
                break;
        }
    }
}

public enum EnumTurtleState
{
    None = -1,
    Move,
    Idle,
    Spin,
}
