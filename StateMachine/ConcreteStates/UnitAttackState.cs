using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitState
{
    private float _timer;
    private Unit _target;
    public LayerMask playerLayer = LayerMask.GetMask("PlayerLayer");

    public UnitAttackState(Unit unit, UnitStateMachine unitStateMachine) : base(unit, unitStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Unit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        _target = unit.GetCurrentTarget().GetComponent<Unit>();
        unit.ClearAggroStatus();
        unit.StopUnit(true);
        unit.MoveUnit(unit.transform.position);
        Vector3 direction = _target.transform.position - unit.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        unit.transform.rotation = rotation;
    }

    public override void ExitState()
    {
        base.ExitState();
        unit.StopUnit(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_timer > unit.UnitData.TimeBetweenAttacks)
        {
            _timer = 0;
            //do the attack
            if (_target != null)
            {
                _target.TakeDamage(unit.UnitData.Damage);
            }
            else
            {
                //target died clear target
                unit.ClearAttackStatusAndTarget();
                //check for new target
                LookForNewTarget();
            }
        }
        _timer += Time.deltaTime;

        if (unit.isAggroed)
        {
            unit.StateMachine.ChangeState(unit.ChaseState);
        }
    }

    private void LookForNewTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(unit.transform.position, unit.UnitData.DetectionRangeRadius, playerLayer);
        Unit bestTarget = null;
        int maxWeight = int.MinValue;

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Unit possibleTarget))
            {
                if (possibleTarget.GetUnitType() == unit.GetFavouriteTarget())
                {
                    unit.SetAggroStatusAndTarget(true, possibleTarget);
                    return;
                }

                TargetPreference targetPreference = unit.GetTargetPreferenceList().Find(T => T.unitType == possibleTarget.GetUnitType());
                if (targetPreference != null && targetPreference.weight > maxWeight)
                {
                    maxWeight = targetPreference.weight;
                    bestTarget = possibleTarget;
                }
            }
        }

        if (bestTarget != null)
        {
            unit.SetAggroStatusAndTarget(true, bestTarget);
        }
        else
        {
            unit.StateMachine.ChangeState(unit.IdleState);
            // failed to find target
        }
    }


    public override void PhysicsUpdateState()
    {
        base.PhysicsUpdateState();
    }
}