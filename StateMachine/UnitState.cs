using UnityEngine;

public class UnitState
{
    protected Unit unit;
    protected UnitStateMachine unitStateMachine;

    public UnitState(Unit unit, UnitStateMachine unitStateMachine)
    {
        this.unit = unit;
        this.unitStateMachine = unitStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void PhysicsUpdateState() { }
    public virtual void AnimationTriggerEvent(Unit.AnimationTriggerType triggerType) { }
}
