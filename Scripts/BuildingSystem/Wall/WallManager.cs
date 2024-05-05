using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightKeepers
{
    public class WallManager : Singleton<WallManager>
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private List<Building> _parentWallsList;
        [SerializeField] private BuildingManager _buildingManager;
        public List<MeshFilter> _wallMeshFilters;

        

        public void CheckEveryWall(Vector2Int gridPosition)
        {
            
        }

        private void CheckEnvironment(Vector2Int gridPosition)
        {    
            if (_gridManager._grid[new Vector2Int(gridPosition.x,gridPosition.y + 1)].building.buildingType == BuildingData.BuildingType.Wall)
            {
                if (_gridManager._grid[new Vector2Int(gridPosition.x + 1,gridPosition.y)].building.buildingType == BuildingData.BuildingType.Wall)
                {
      
                }
            }
            
        }
    }
}
