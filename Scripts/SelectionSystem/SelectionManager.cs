using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NightKeepers
{
    public class SelectionManager : Singleton<SelectionManager>
    {
        private Transform highlight;
        private RaycastHit raycastHit;

        private Building selectedBuilding;

        public static event Action<FunctionalBuilding> onBuildingSelected;  

        private void Update() {

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                Vector2Int gridPosition = GridManager.Instance._grid.WorldToGridPosition(raycastHit.point);

                OutlineSelection();
                SelectedBuilding(gridPosition);
                DeleteBuilding(gridPosition);
            }
        }

        private void SelectedBuilding(Vector2Int gridPosition)
        {
            if (Input.GetMouseButtonDown(0) && GridManager.Instance._grid[gridPosition].building != null)
            {
                if (GridManager.Instance._grid[gridPosition].building.TryGetComponent<FunctionalBuilding>(out var func))
                {
                    onBuildingSelected?.Invoke(func);
                }
            }
        }

        private void OutlineSelection()
        {
            if (highlight != null)
            {
                highlight.gameObject.GetComponent<Outline>().enabled = false;
                highlight = null;
            }
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) 
            {
                highlight = raycastHit.transform;
                if (highlight.CompareTag("Selectable"))
                {
                    if (highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
                        highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                    }
                }
                else
                {
                    highlight = null;
                }
            }        
        }                  

        private void DeleteBuilding(Vector2Int gridPosition)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Tile tile = new Tile()
                {
                    building = null,
                    tileType = GridManager.Instance._grid[gridPosition].tileType
                };

                if (GridManager.Instance._grid[gridPosition].building != null)
                {
                    Destroy(GridManager.Instance._grid[gridPosition].building.gameObject);
                    foreach (var item in GridManager.Instance._grid[gridPosition].building.buildingData.GetGridPositionList(gridPosition, GridManager.Instance._grid[gridPosition].building.direction))
                    {
                        GridManager.Instance._grid[item] = tile;
                    }
                }                
            }
        }
    }
}
