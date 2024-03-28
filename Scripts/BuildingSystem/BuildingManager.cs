using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private List<Building> building;
    private BuildingData.BuildingType buildingType;
    private bool canBuild;

    private int buildingNumber;

    private void Update() 
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        SelectBuilding();
        if (Input.GetMouseButton(0))
        {
            PlaceBuilding(buildingNumber);
        }
    }

    private void SelectBuilding()
    {
        switch (buildingType)
        {
            case BuildingData.BuildingType.StoneMine:
                buildingNumber = 0;
            break;

            case BuildingData.BuildingType.IronMine:
                buildingNumber = 1;
            break;

            case BuildingData.BuildingType.Lumberjack:
                buildingNumber = 2;
            break;

            case BuildingData.BuildingType.TownHall:
                buildingNumber = 3;
            break;

            case BuildingData.BuildingType.Test:
                buildingNumber = 4;
            break;
        }
    }

    private void PlaceBuilding(int count)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            Vector2Int gridPosition = _gridManager._grid.WorldToGridPosition(raycastHit.point);
            List<Vector2Int> gridPositionList = building[count].buildingData.GetGridPositionList(gridPosition, building[count].direction);
            
            bool canBuild = true;

            foreach (Vector2Int position in gridPositionList)
            {
                if (_gridManager._grid[position].building != null)
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = building[count].buildingData.GetRotationOffset(building[count].direction);
                Vector3 instantiatedBuildingWorldPosition = _gridManager._grid.GridToWorldPosition(gridPosition) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _gridManager.cellSize;
                Building instantiatedBuilding = Instantiate(
                        building[count],
                        instantiatedBuildingWorldPosition, 
                        Quaternion.Euler(0, building[count].buildingData.GetRotationAngle(building[count].direction), 0));
                
                foreach (Vector2Int position in gridPositionList)
                {
                    Tile tile = new Tile()
                    {
                        building = instantiatedBuilding
                    };
                    _gridManager._grid[position] = tile;
                }
            }
        }
    }

    public void SetBuildingType(BuildingData.BuildingType buildingType)
    {
        this.buildingType = buildingType;
    }
}
