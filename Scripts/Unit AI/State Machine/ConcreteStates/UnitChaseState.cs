using UnityEngine;

public class UnitChaseState : UnitState
{
    private Vector3 _previousTargetPosition;

    public UnitChaseState(Unit unit, UnitStateMachine unitStateMachine) : base(unit, unitStateMachine)
    {

    }

    public override void EnterState()
    {
        unit.currentStateName = "Chase";
        base.EnterState();
        unit._animation.CrossFade("UndeadRun1");
    }

    public override void ExitState()
    {
        base.ExitState();
        unit.ClearAggroStatus();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!unit.GetCurrentTargetUnit())
        {
            unit.LookForNewChaseTarget();
        }

        if (unit.isInAttackingDistance)
        {
            unit.StateMachine.ChangeState(unit.AttackState);
        }
    }

    public override void PhysicsUpdateState()
    {
        base.PhysicsUpdateState();

        if (unit.GetCurrentTargetUnit() && DidTargetMove(1f))
        {
            unit.MoveUnit(unit.GetValidPositionAroundTarget());
        }
    }

    private bool DidTargetMove(float threshold)
    {
        Vector3 previousPosition = _previousTargetPosition;
        Vector3 currentPosition = unit.GetCurrentTargetPosition();

        float distance = Vector3.Distance(previousPosition, currentPosition);

        if (distance <= threshold)
        {
            return false;
        }

        _previousTargetPosition = currentPosition;
        return true;
    }
}