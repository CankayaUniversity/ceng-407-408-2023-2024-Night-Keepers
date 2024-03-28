using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public BuildingData.Dir direction = BuildingData.Dir.Down;
    public BuildingData.BuildingType buildingType = BuildingData.BuildingType.Empty;

}
