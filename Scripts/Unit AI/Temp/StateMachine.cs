using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<Estate> : MonoBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();

    protected BaseState<Estate> CurrentState;

    private void Start()
    {
        CurrentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter();
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}