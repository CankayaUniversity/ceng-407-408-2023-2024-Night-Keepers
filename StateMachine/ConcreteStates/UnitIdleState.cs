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
        base.EnterState();
        if (unit.UnitData.Side != UnitSide.Enemy) return;
        if (EnemySpawnManager.Instance.playerBaseList.Count > 0 && EnemySpawnManager.Instance.playerBaseList[0] != null)
        {
            // for now it only picks the first base in the list later we will have to create a logic to pick one and spawn enemies according to that and pick the base according to that
            _targetBasePosition = EnemySpawnManager.Instance.playerBaseList[0].transform.position;

            Vector3 directionToTargetBase = (_targetBasePosition - unit.transform.position).normalized;
            float distanceToTargetBase = Vector3.Distance(unit.transform.position, _targetBasePosition);
            Vector3 straightLinePosition = unit.transform.position + directionToTargetBase * (distanceToTargetBase * 0.9f);
            unit.MoveUnit(straightLinePosition);
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
        base.PhysicsUpdateState();
    }
}