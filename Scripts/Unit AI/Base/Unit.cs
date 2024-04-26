using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnitScriptableObject;

public class Unit : MonoBehaviour, IDamageable, IMoveable, ITriggerCheckable
{
    [field: SerializeField] public string currentStateName { get; set; } = "Idle";

    public int CurrentHealth { get; set; }

    public UnitStateMachine StateMachine { get; set; }
    public UnitIdleState IdleState { get; set; }
    public UnitChaseState ChaseState { get; set; }
    public UnitAttackState AttackState { get; set; }

    [field: SerializeField]public bool isAggroed { get; set; }
    [field: SerializeField]public bool isInAttackingDistance { get; set; }

    private TargetPreference _favouriteTarget;

    [field: SerializeField] private GameObject Target;
    private Vector3 TargetPosition;
    private Unit CurrentTargetUnit;
    private NavMeshAgent navAgent;

    NavMeshPath tempPath;

    [field: SerializeField] public UnitScriptableObject UnitData { get; set; }

    public static event Action onBuildingDestroyed;

    [HideInInspector]
    public LayerMask playerLayer;

    private void Awake()
    {
        TryGetComponent(out NavMeshAgent agent);
        navAgent = agent;

        StateMachine = new UnitStateMachine();
        IdleState = new UnitIdleState(this, StateMachine);
        ChaseState = new UnitChaseState(this, StateMachine);
        AttackState = new UnitAttackState(this, StateMachine);

        playerLayer = LayerMask.GetMask("PlayerLayer");
    }

    private void Start()
    {
        CurrentHealth = UnitData.MaxHealth;

        if (GetUnitType() != UnitType.Building)
        {
            navAgent.speed = UnitData.movementSpeed;
        }

        FindFavouriteTarget();

        StateMachine.Initialize(IdleState);

        tempPath = new NavMeshPath();
    }

    private void Update()
    {
        StateMachine.CurrentUnitState.UpdateState();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentUnitState.PhysicsUpdateState();
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0 )
        {
            Die();
        }
    }

    public void Die()
    {
        if (UnitData.Type == UnitType.Building)
        {
            onBuildingDestroyed?.Invoke();
        }
        Destroy(gameObject);
    }

    public void MoveUnit(Vector3 destination)
    {
        //RB.velocity = velocity;
        navAgent.SetDestination(destination);
        //RotateUnit(velocity);
    }

    public void StopUnit(bool stop)
    {
        navAgent.isStopped = stop;
    }

    public enum AnimationTriggerType
    {
        UnitAttack,
        UnitDamaged,
    }

    public UnitType GetUnitType()
    {
        return UnitData.Type;
    }

    public int GetUnitPowerPoints()
    {
        return UnitData.UnitPowerPoints;
    }

    public List<TargetPreference> GetTargetPreferenceList()
    {
        return UnitData.targetPreferences;
    }

    public void FindFavouriteTarget()
    {
        int maxValue = int.MinValue;
        foreach (var item in GetTargetPreferenceList())
        {
            if (item.weight > maxValue)
            {
                maxValue = item.weight;
                _favouriteTarget = item;
            }
        }
    }

    public UnitType GetFavouriteTarget()
    {
        return _favouriteTarget.unitType;
    }

    public int GetFavouriteTargetWeight()
    {
        return _favouriteTarget.weight;
    }

    public GameObject GetCurrentTarget()
    {
        return Target;
    }

    public UnitType GetTypeOfOtherUnit(Unit unit)
    {
        return unit.UnitData.Type;
    }

    public Vector3 GetCurrentTargetPosition()
    {
        return TargetPosition;
    }

    public UnitType GetCurrentTargetUnitType()
    {
        return CurrentTargetUnit.GetUnitType();
    }

    public int GetCurrentTargetWeight()
    {
        TargetPreference targetPreference = GetTargetPreferenceList().Find(T => T.unitType == GetCurrentTargetUnitType());

        return targetPreference != null ? targetPreference.weight : 0;
    }

    public bool IsNewTargetBetter(UnitType newTargetType)
    {
        if (newTargetType == GetFavouriteTarget() && GetCurrentTargetUnitType() != GetFavouriteTarget())
        {
            return true;
        }

        TargetPreference targetPreference = GetTargetPreferenceList().Find(T => T.unitType == newTargetType);

        return targetPreference.weight > GetCurrentTargetWeight() ? true : false;
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentUnitState.AnimationTriggerEvent(triggerType);
    }

    public void SetAggroStatusAndTarget(bool isAggroed, Unit target)
    {
        if (IsTargetReachable(target))
        {
            this.isAggroed = isAggroed;
            Target = target.gameObject;
            TargetPosition = target.transform.position;
            CurrentTargetUnit = target;
        }
    }

    public void ClearAggroStatus()
    {
        isAggroed = false;
    }   

    public void ClearAttackStatusAndTarget()
    {
        isInAttackingDistance = false;
        Target = null;
        TargetPosition = Vector3.zero;
    }

    public void SetAttackingStatus(bool isInAttackingDistance)
    {
        this.isInAttackingDistance = isInAttackingDistance;
    }

    public Vector3 GetValidPositionAroundTarget()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
        randomDirection += GetCurrentTargetPosition();

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas);

        return hit.position;
    }


    public bool IsTargetReachable(Unit unit)
    {
        if (GetTypeOfOtherUnit(unit) == UnitType.Building)
        {
            return navAgent.CalculatePath(unit.transform.position, tempPath);
        }
        navAgent.CalculatePath(unit.transform.position, tempPath);
        return  tempPath.status == NavMeshPathStatus.PathComplete;
    }

    public void LookForNewTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, UnitData.DetectionRangeRadius, playerLayer);
        Unit bestTarget = null;
        int maxWeight = int.MinValue;

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Unit possibleTarget))
            {
                if (possibleTarget.GetUnitType() == GetFavouriteTarget())
                {
                    SetAggroStatusAndTarget(true, possibleTarget);
                    return;
                }

                TargetPreference targetPreference = GetTargetPreferenceList().Find(T => T.unitType == possibleTarget.GetUnitType());
                if (targetPreference != null && targetPreference.weight > maxWeight)
                {
                    maxWeight = targetPreference.weight;
                    bestTarget = possibleTarget;
                }
            }
        }

        if (bestTarget != null)
        {
            SetAggroStatusAndTarget(true, bestTarget);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
            // failed to find target
        }
    }
}