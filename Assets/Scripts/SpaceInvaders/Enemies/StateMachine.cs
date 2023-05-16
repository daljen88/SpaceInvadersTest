using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EnemyState currentState;

    public void ChangeState(EnemyState state)
    {
        string currStateValue = "null";

        if(currentState != null)
        { currStateValue = currentState.EnState.ToString(); }

        Debug.Log($"change state {currStateValue} - to {state.EnState}");
        currentState?.Exit();
        currentState = state;
        currentState.Inizialize(currentState);
    }

    public void Inizialize()
    {
        return;
    }
    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(currentState);
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
