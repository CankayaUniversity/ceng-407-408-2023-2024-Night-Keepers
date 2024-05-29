using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace NightKeepers
{
    public class Wall : MonoBehaviour
    {
        public Wall UpWall;
        public Wall DownWall;
        public Wall LeftWall;
        public Wall RightWall;

        [SerializeField] private bool isPreview = false;

        private Vector3 cornerOffset = new Vector3(1.672363f, 0, -1.672364f);
        private Vector3 tripleOffset = new Vector3(1.672363f, 0, 1.490116e-07f);

        private MeshFilter meshFilter;
        [SerializeField] private Transform childTransform;
        public bool Up, Down, Left, Right;

        public bool isVertical;

        private void Awake() {
            meshFilter = GetComponentInChildren<MeshFilter>();
        }

        private void Start() {

            if (isPreview) {
                return;
            }

            CheckVertical();
            CheckAllSides();
            
            if (UpWall != null)
            {
                UpWall.CheckVertical();
                UpWall.CheckAllSides();
            }

            if (DownWall != null)
            {
                DownWall.CheckVertical();
                DownWall.CheckAllSides();

            }

            if (LeftWall != null)
            {
                LeftWall.CheckVertical();
                LeftWall.CheckAllSides();
            }

            if (RightWall != null)
            {
                RightWall.CheckVertical();
                RightWall.CheckAllSides();
            }
        }

        public void CheckAllSides()
        {
            if (!GridManager.Instance._grid.IsInDimensions(GridManager.Instance._grid.WorldToGridPosition(transform.position)))
            {
                return;
            }
            
            CheckUp(GridManager.Instance._grid.WorldToGridPosition(transform.position));
            CheckDown(GridManager.Instance._grid.WorldToGridPosition(transform.position));
            CheckLeft(GridManager.Instance._grid.WorldToGridPosition(transform.position));
            CheckRight(GridManager.Instance._grid.WorldToGridPosition(transform.position));
            UpdateWall();
        }

        public void CheckVertical()
        {
            if (transform.rotation == Quaternion.Euler(0, 0, 0) || transform.rotation == Quaternion.Euler(0, 180, 0))
            {
                isVertical = true;                
            }
            if (transform.rotation == Quaternion.Euler(0, 270, 0) || transform.rotation == Quaternion.Euler(0, 90, 0))
            {
                isVertical = false;
            }
        }
        

        public void CheckUp(Vector2Int gridPosition)
        {
            if (GridManager.Instance._grid[new Vector2Int(gridPosition.x, gridPosition.y + 1)].building != null && GridManager.Instance._grid[new Vector2Int(gridPosition.x, gridPosition.y + 1)].building.buildingType == BuildingData.BuildingType.Wall)
            {
                UpWall = GridManager.Instance._grid[new Vector2Int(gridPosition.x, gridPosition.y + 1)].building.GetComponent<Wall>();
                Up = true;
            }
            else
            {
                Up = false; 
            }
            
            
        }

        public void CheckDown(Vector2Int gridPosition)
        {
            if (GridManager.Instance._grid[new Vector2Int(gridPosition.x, gridPosition.y - 1)].building != null && GridManager.Instance._grid[new Vector2Int(gridPosition.x, gridPosition.y - 1)].building.buildingType == BuildingData.BuildingType.Wall)
            {
                DownWall = GridManager.Instance._grid[new Vector2Int(gridPosition.x, gridPosition.y - 1)].building.GetComponent<Wall>();
                Down = true;
            }
            else
            {
                Down = false; 
            }

        }

        public void CheckLeft(Vector2Int gridPosition)
        {
            if (GridManager.Instance._grid[new Vector2Int(gridPosition.x - 1, gridPosition.y)].building != null && GridManager.Instance._grid[new Vector2Int(gridPosition.x - 1, gridPosition.y)].building.buildingType == BuildingData.BuildingType.Wall)
            {
                LeftWall = GridManager.Instance._grid[new Vector2Int(gridPosition.x - 1, gridPosition.y)].building.GetComponent<Wall>();
                Left = true;
            }
            else
            {
                Left = false; 
            }

        }

        public void CheckRight(Vector2Int gridPosition)
        {
            if (GridManager.Instance._grid[new Vector2Int(gridPosition.x + 1, gridPosition.y)].building != null && GridManager.Instance._grid[new Vector2Int(gridPosition.x + 1, gridPosition.y)].building.buildingType == BuildingData.BuildingType.Wall)
            {
                RightWall = GridManager.Instance._grid[new Vector2Int(gridPosition.x + 1, gridPosition.y)].building.GetComponent<Wall>();
                Right = true;
            }
            else
            {
                Right = false; 
            }

        }

        public void UpdateWall()
        {
            if (Up && Right)
            {
                if (UpWall.isVertical && !RightWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[0].sharedMesh;
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    childTransform.transform.localPosition = cornerOffset;

                    if (RightWall.UpWall == null)
                    {
                        isVertical = false;
                    }
                }
            }

            if (Up && Left)
            {
                if (UpWall.isVertical && !LeftWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[0].sharedMesh;
                    transform.rotation = Quaternion.Euler(0 , 180, 0);        
                    childTransform.transform.localPosition = cornerOffset;

                    if (LeftWall.UpWall == null)
                    {
                        isVertical = false;
                    }

                }
            }
            
            if (Down && Right)
            {     
                if (DownWall.isVertical && !RightWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[0].sharedMesh;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    childTransform.transform.localPosition = cornerOffset;

                    if (RightWall.DownWall == null)
                    {
                        isVertical = false;
                    }

                }
            }

            if (Down && Left)
            {
                if (DownWall.isVertical && !LeftWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[0].sharedMesh;
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    childTransform.transform.localPosition = cornerOffset;
                
                    if (LeftWall.DownWall == null)
                    {
                        isVertical = false;
                    }

                }
            }

            if (Up && Right && Left)
            {
                if (UpWall.isVertical && !RightWall.isVertical && !LeftWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[1].sharedMesh;
                    transform.rotation = Quaternion.Euler(0 , 270, 0);
                    childTransform.transform.localPosition = new Vector3(1.672363f, 0, -3.829598e-05f);
                }
            }

            if (Up && Right && Down)
            {
                if (UpWall.isVertical && DownWall.isVertical && !RightWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[1].sharedMesh;
                    childTransform.transform.localPosition = tripleOffset;
                }
            }

            if (Right && Down && Left)
            {
                if (DownWall.isVertical && !LeftWall.isVertical && !RightWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[1].sharedMesh;
                    childTransform.transform.localPosition = tripleOffset;
                }
            }

            if (Down && Left && Up)
            {
                if (UpWall.isVertical && DownWall.isVertical && !LeftWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[1].sharedMesh;
                    transform.rotation = Quaternion.Euler(0 , 180, 0);
                    childTransform.transform.localPosition = tripleOffset;
                }   
            }

            if (Up && Down && Right && Left)
            {
                if (UpWall.isVertical && DownWall.isVertical && !RightWall.isVertical && !LeftWall.isVertical)
                {
                    meshFilter.mesh = WallManager.Instance._wallMeshFilters[2].sharedMesh;
                    childTransform.transform.localPosition = new Vector3(3.790855e-05f , 0 , 2.812286e-09f);
                }
            }
        }
    }
}
