using UnityEngine;

public class UnitChaseState : UnitState
{
    private Vector3 _previousTargetPosition;

    public UnitChaseState(Unit unit, UnitStateMachine unitStateMachine) : base(unit, unitStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Unit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        unit.currentStateName = "Chase";
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        unit.ClearAggroStatus();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!unit.GetCurrentTarget())
        {
            unit.LookForNewTarget();
        }

        if (unit.isInAttackingDistance)
        {
            unit.StateMachine.ChangeState(unit.AttackState);
        }

        if (DidTargetMove())
        {
            unit.MoveUnit(unit.GetValidPositionAroundTarget());
        }
    }

    public override void PhysicsUpdateState()
    {
        base.PhysicsUpdateState();
    }

    private bool DidTargetMove()
    {
        if (_previousTargetPosition == unit.GetCurrentTargetPosition())
        {
            return false;
        }
        _previousTargetPosition = unit.GetCurrentTargetPosition();
        return true;
    }
}