using NightKeepers.Research;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingData;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;
    public Upgrades upgrades;
    public void Lumberjack()
    {
        //buildingManager.SetBuildingType(BuildingType.Lumberjack);
         if (upgrades.unlockedUpgrades.Contains(Upgrades.ResearchUpgrades.Lumberjack2))
         {
             buildingManager.SetBuildingType(BuildingType.Lumberjack2);
             Debug.Log("LB2");
         }
         if (upgrades.unlockedUpgrades.Contains(Upgrades.ResearchUpgrades.Lumberjack1))
         {
             buildingManager.SetBuildingType(BuildingType.Lumberjack1);
             Debug.Log("LB1");
         }
         else
         {
            
             Debug.Log("LB");
         }

    }
    public void StoneMine()
    {
       // buildingManager.SetBuildingType(BuildingData.BuildingType.StoneMine);

         if (upgrades.unlockedUpgrades.Contains(Upgrades.ResearchUpgrades.StoneMine))
         {
             Debug.Log("STONEMINE");
         }
         else
             Debug.Log("Cannot build StoneMine, Research first!");
    }

    public void IronMine()
    {
        if (upgrades.unlockedUpgrades.Contains(Upgrades.ResearchUpgrades.IronMine))
        {
            buildingManager.SetBuildingType(BuildingData.BuildingType.IronMine);
            Debug.Log("IRONMINE");
        }
        else
            Debug.Log("Cannot build IronMine, Research first!");
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

        if (upgrades.unlockedUpgrades.Contains(Upgrades.ResearchUpgrades.Wall))
            {
            buildingManager.SetBuildingType(BuildingData.BuildingType.Wall);
            Debug.Log("WALL");
            }
         else
             Debug.Log("Cannot build Wall, Research first!");        
    }

    public void House()
    {
        buildingManager.SetBuildingType(BuildingData.BuildingType.House);
    }
}
