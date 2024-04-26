using UnityEngine;

public class UnitAttackState : UnitState
{
    private float _timer;
    private Unit _target;

    public UnitAttackState(Unit unit, UnitStateMachine unitStateMachine) : base(unit, unitStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Unit.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        unit.currentStateName = "Attack";
        base.EnterState();
        _target = unit.GetCurrentTarget().GetComponent<Unit>();
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

        if (!unit.GetCurrentTarget())
        {
            //target died clear target
            unit.ClearAttackStatusAndTarget();
            //check for new target
            unit.LookForNewTarget();
        }

        if (_timer > unit.UnitData.TimeBetweenAttacks)
        {
            _timer = 0;
            //do the attack
            if (_target != null)
            {
                _target.TakeDamage(unit.UnitData.Damage);
            }
        }
        _timer += Time.deltaTime;

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