using UnityEngine;

public class UnitStateMachine
{
    public UnitState CurrentUnitState { get; set; }

    public void Initialize(UnitState startingState)
    {
        CurrentUnitState = startingState;
        CurrentUnitState.EnterState();
    }

    public void ChangeState(UnitState newState) 
    {
        CurrentUnitState.ExitState();
        CurrentUnitState = newState;
        CurrentUnitState.EnterState();
    }
}