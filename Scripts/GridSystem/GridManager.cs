using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellSize;
    [SerializeField] private List<Building> building;

    private External _external;
    private Grid<Tile> _grid;
    private TextMesh[,] _debugText;
    public const int sortingOrderDefault = 5000;
    private void Awake()
    {
        _grid = new Grid<Tile>(width, height, cellSize);
        DebugLines();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                // int randomNumber = Random.Range(0, building.Count);
                // Vector2Int gridPosition = _grid.WorldToGridPosition(raycastHit.point);
                //
                // List<Vector2Int> gridPositionList = building[4].buildingData.GetGridPositionList(gridPosition, building[4].direction);
                //
                // bool canBuild = true;
                //
                // foreach (Vector2Int position in gridPositionList)
                // {
                //     if (_grid[position].building != null)
                //     {
                //         canBuild = false;
                //         break;
                //     }
                // }
                //
                // if (canBuild)
                // {
                //     Vector2Int rotationOffset = building[4].buildingData.GetRotationOffset(building[4].direction);
                //     Vector3 instantiatedBuildingWorldPosition = _grid.GridToWorldPosition(gridPosition) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * cellSize;
                //     Building instantiatedBuilding = 
                //         Instantiate(
                //             building[4],
                //             instantiatedBuildingWorldPosition, 
                //             Quaternion.Euler(0, building[4].buildingData.GetRotationAngle(building[4].direction), 0));
                //     foreach (Vector2Int position in gridPositionList)
                //     {
                //         Tile tile = new Tile()
                //         {
                //             building = instantiatedBuilding
                //         };
                //     
                //         _grid[position] = tile;
                //     }
                // }

                Vector2Int gridPosition = _grid.WorldToGridPosition(raycastHit.point);
                _grid._grid[gridPosition.x, gridPosition.y].tileType = TileType.Water;
                _debugText[gridPosition.x, gridPosition.y].text = _grid._grid[gridPosition.x, gridPosition.y].tileType.ToString();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            building[4].direction = BuildingData.GetNextDir(building[4].direction);
        }
    }

    private void DebugLines()
    {
        _debugText = new TextMesh[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                _debugText[x,z] = CreateWorldText(_grid._grid[x,z].tileType.ToString(), null, _grid.GridToWorldPosition(new Vector2Int(x, z)), 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(_grid.GridToWorldPositionDrawLine(new Vector2Int(x, z)), _grid.GridToWorldPositionDrawLine(new Vector2Int(x, z + 1)), Color.white, 100f);
                Debug.DrawLine(_grid.GridToWorldPositionDrawLine(new Vector2Int(x, z)), _grid.GridToWorldPositionDrawLine(new Vector2Int(x + 1, z)), Color.white, 100f);
            }
        }
        Debug.DrawLine(_grid.GridToWorldPositionDrawLine(new Vector2Int(0, height)), _grid.GridToWorldPositionDrawLine(new Vector2Int(width, height)) , Color.white, 100f);
        Debug.DrawLine(_grid.GridToWorldPositionDrawLine(new Vector2Int(width, 0)), _grid.GridToWorldPositionDrawLine(new Vector2Int(width, height)), Color.white, 100f);
    }
    
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.rotation = Quaternion.Euler(90f, 0, 0);
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
