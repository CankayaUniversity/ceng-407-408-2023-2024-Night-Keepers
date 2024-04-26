using System;
using UnityEngine;

public abstract class BaseState<Estate> where Estate : Enum
{
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract Estate GetNextState();
    public abstract void OnTriggerEnter();
    public abstract void OnTriggerStay();
    public abstract void OnTriggerExit();
}