using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private AIController aiController;
    private float idleDuration = 2f;
    private float idleTimer;

    public StateType Type => StateType.Idle;

    public IdleState(AIController aiController)
    {
        this.aiController = aiController;
    }

    public void Enter()
    {
        idleTimer = 0f;
        
    }

    public void Execute()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            aiController.StateMachine.TransitionToState(
                StateType.Patrol
            );
        }
    }

    public void Exit()
    {
    }
}
