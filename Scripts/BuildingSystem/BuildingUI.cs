using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;

    public void Lumberjack()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.Lumberjack);
    }

    public void StoneMine()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.StoneMine);
    }

    public void IronMine()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.IronMine);
    }

    public void TownHall()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.TownHall);
    }

    public void Test()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.Test);
    }

    public void Wall()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.Wall);
    }

    public void House()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.House);
    }
}
