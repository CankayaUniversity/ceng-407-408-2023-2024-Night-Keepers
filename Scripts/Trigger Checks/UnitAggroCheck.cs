using UnityEngine;
using static UnitScriptableObject;

public class UnitAggroCheck : MonoBehaviour
{
    private Unit _thisUnit;

    private void Awake()
    {
        _thisUnit = GetComponentInParent<Unit>();
    }

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _thisUnit.UnitData.DetectionRangeRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_thisUnit.UnitData.Side)
        {
            case UnitSide.Enemy:
                switch (_thisUnit.StateMachine.CurrentUnitState)
                {
                    case UnitIdleState:
                        if (other.CompareTag("EnemyUnit")) return;
                        if (!other.transform.TryGetComponent(out Unit newTarget)) return;

                        _thisUnit.SetAggroStatusAndTarget(true, newTarget);
                        break;
                    case UnitChaseState:
                        if (other.CompareTag("EnemyUnit")) return;
                        if (!other.transform.TryGetComponent(out Unit chasingNewTarget)) return;

                        if (_thisUnit.IsNewTargetBetter(chasingNewTarget.GetUnitType()))
                        {
                            _thisUnit.SetAggroStatusAndTarget(true, chasingNewTarget);
                        }
                        break;
                    case UnitAttackState:
                        if (other.CompareTag("EnemyUnit")) return;
                        if (!other.transform.TryGetComponent(out Unit attackingNewTarget)) return;

                        if (_thisUnit.IsNewTargetBetter(attackingNewTarget.GetUnitType()))
                        {
                            _thisUnit.ClearAttackStatusAndTarget();
                            _thisUnit.SetAggroStatusAndTarget(true, attackingNewTarget);
                        }
                        break;
                }
                break;
            case UnitSide.Player:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // keep chasing even if they exit so dont do anything here for now
    }
}