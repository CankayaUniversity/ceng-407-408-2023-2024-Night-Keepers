using System.Collections.Generic;
using UnityEngine;
using static UnitScriptableObject;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitScriptableObject", order = 1)]
public class UnitScriptableObject : ScriptableObject
{
    public int MaxHealth = 100;
    public int Damage = 0;
    public float TimeBetweenAttacks = 0;
    public float DetectionRangeRadius = 0f;
    public float AttackRangeRadius = 0f;
    public float movementSpeed = 0f;
    public List<TargetPreference> targetPreferences = new List<TargetPreference>();

    public int UnitPowerPoints = 1;

    public enum UnitSide
    {
        Player,
        Enemy,
    }
    public UnitSide Side;

    public enum UnitType
    {
        Building,
        Melee,
        Ranged,
    }
    public UnitType Type;

}

[System.Serializable]
public class TargetPreference
{
    public UnitType unitType;
    public int weight;

    //public override bool Equals(object obj)
    //{
    //    if (obj == null || GetType() != obj.GetType())
    //        return false;

    //    TargetPreference other = (TargetPreference)obj;
    //    return unitType == other.unitType;
    //}

    //public override int GetHashCode()
    //{
    //    return unitType.GetHashCode();
    //}
}