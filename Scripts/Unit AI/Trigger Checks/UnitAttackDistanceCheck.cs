using UnityEngine;

public class UnitAttackDistanceCheck : MonoBehaviour
{
    private Unit _thisUnit;

    private void Awake()
    {
        _thisUnit = GetComponentInParent<Unit>();
    }

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _thisUnit.UnitData.AttackRangeRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_thisUnit.GetCurrentTargetObject() == other.gameObject)
        {
            _thisUnit.SetAttackingStatus(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_thisUnit.GetCurrentTargetObject() == other.gameObject)
        {
            _thisUnit.SetAttackingStatus(false);
            _thisUnit.SetAggroStatusAndTarget(true, _thisUnit.GetCurrentTargetUnit());
        }
    }
}