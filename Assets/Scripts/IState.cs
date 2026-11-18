using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState
{
    StateType Type { get; }

    void Enter();
    void Execute();
    void Exit();
}
