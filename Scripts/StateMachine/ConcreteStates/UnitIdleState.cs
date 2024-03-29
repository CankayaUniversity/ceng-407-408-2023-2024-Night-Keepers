using UnityEngine;
using static UnitScriptableObject;

public class UnitIdleState : UnitState
{
    private Vector3 _targetBasePosition;

    public UnitIdleState(Unit unit, UnitStateMachine unitStateMachine) : base(unit, unitStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Unit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        unit.currentStateName = "Idle";
        base.EnterState();
        if (unit.UnitData.Side != UnitSide.Enemy) return;

        Vector3 directionToTargetBase = (_targetBasePosition - unit.transform.position).normalized;
        float distanceToTargetBase = Vector3.Distance(unit.transform.position, _targetBasePosition);
        Vector3 straightLinePosition = unit.transform.position + directionToTargetBase * (distanceToTargetBase * 0.9f);
        unit.MoveUnit(straightLinePosition);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (unit.isAggroed)
        {
            unit.StateMachine.ChangeState(unit.ChaseState);
        }
    }

    public override void PhysicsUpdateState()
    {
        base.PhysicsUpdateState();
    }
}