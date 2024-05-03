using Unity.IO.LowLevel.Unsafe;
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
            unit._animation.CrossFade("SoldierIdle1");
        }
        else
        {
            _targetBasePosition = PlayerBaseManager.Instance.GetSelectedBasePosition();
            Vector3 directionToTargetBase = (_targetBasePosition - unit.transform.position).normalized;
            float distanceToTargetBase = Vector3.Distance(unit.transform.position, _targetBasePosition);
            Vector3 straightLinePosition = unit.transform.position + directionToTargetBase * (distanceToTargetBase * 0.9f);
            unit.MoveUnit(straightLinePosition);
            unit._animation.Play("UndeadRun1");
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
        if (unit.UnitData.Side == UnitSide.Player && unit.GetUnitType() != UnitType.Building)
        {
            Collider[] enemyColliders = Physics.OverlapSphere(unit.transform.position, unit.UnitData.DetectionRangeRadius, unit.enemyLayer);
            Unit bestEnemyTarget = null;
            int maxEnemyWeight = int.MinValue;

            foreach (Collider col in enemyColliders)
            {
                if (col.TryGetComponent(out Unit possibleTarget))
                {
                    if (possibleTarget.GetUnitType() == unit.GetFavouriteTarget())
                    {
                        unit.SetAggroStatusAndTarget(true, possibleTarget);
                        return;
                    }

                    TargetPreference targetPreference = unit.GetTargetPreferenceList().Find(T => T.unitType == possibleTarget.GetUnitType());
                    if (targetPreference != null && targetPreference.weight > maxEnemyWeight)
                    {
                        maxEnemyWeight = targetPreference.weight;
                        bestEnemyTarget = possibleTarget;
                    }
                }
            }

            if (bestEnemyTarget != null)
            {
                unit.SetAggroStatusAndTarget(true, bestEnemyTarget);
            }
            else
            {
                unit.StateMachine.ChangeState(unit.IdleState);
                // failed to find target
            }
        }
        base.PhysicsUpdateState();
    }
}