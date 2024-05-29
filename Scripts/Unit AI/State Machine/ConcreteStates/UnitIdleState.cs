using UnityEngine;
using static UnitScriptableObject;

public class UnitIdleState : UnitState
{
    private Vector3 _targetBasePosition;

    public UnitIdleState(Unit unit, UnitStateMachine unitStateMachine) : base(unit, unitStateMachine)
    {

    }

    public override void EnterState()
    {
        unit.currentStateName = "Idle";
        base.EnterState();

        if (unit.GetUnitType() == UnitType.Building) return;

        if (unit.UnitData.Side != UnitSide.Enemy)
        {
            unit.Animation.CrossFade(unit.AnimationNames[0]);
        }
        else
        {
            _targetBasePosition = PlayerBaseManager.Instance.GetSelectedBasePosition();
            Vector3 directionToTargetBase = (_targetBasePosition - unit.transform.position).normalized;
            float distanceToTargetBase = Vector3.Distance(unit.transform.position, _targetBasePosition);
            Vector3 straightLinePosition = unit.transform.position + directionToTargetBase * (distanceToTargetBase * 0.9f);
            unit.MoveUnit(straightLinePosition);
            unit.Animation.CrossFade(unit.AnimationNames[1]);
        }
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
        if (unit.GetUnitType() != UnitType.Building)
        {
            unit.LookForNewChaseTarget();
        }
        base.PhysicsUpdateState();
    }
}