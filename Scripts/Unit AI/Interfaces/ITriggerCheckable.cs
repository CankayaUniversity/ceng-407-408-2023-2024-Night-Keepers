using UnityEngine;

public interface ITriggerCheckable
{
    bool isAggroed { get; set; }
    bool isInAttackingDistance { get; set; }

    void SetAggroStatusAndTarget(bool isAggroed, Unit target);
    void SetAttackingStatus(bool isInAttackingDistance);
}