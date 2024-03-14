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

    [SerializeField] private int grassProbability;
    [SerializeField] private int rockProbability;
    [SerializeField] private int waterProbability;
    [SerializeField] private int woodProbability;
    [SerializeField] private int ironProbability;

    [SerializeField] private int grassPropagation;
    [SerializeField] private int rockPropagation;
    [SerializeField] private int waterPropagation;
    [SerializeField] private int woodPropagation;
    [SerializeField] private int ironPropagation;

    [SerializeField] private float grassNoise;
    [SerializeField] private float rockNoise;
    [SerializeField] private float waterNoise;
    [SerializeField] private float woodNoise;
    [SerializeField] private float ironNoise;

    [SerializeField] private List<Building> building;
    [SerializeField] private List<GameObject> tilePrefabs;

    private bool isGenerated = false;

    private External _external;
    private Grid<Tile> _grid;
    private TextMesh[,] _debugText;
    public const int sortingOrderDefault = 5000;
    private const int batchInstantiateSize = 1000;
    private void Awake()
    {
        _grid = new Grid<Tile>(width, height, cellSize);
        DebugLines();
        InstantiateMap();
    }

    private void Update()
    {  

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                Vector2Int gridPosition = _grid.WorldToGridPosition(raycastHit.point);
                Debug.Log(_grid._grid[gridPosition.x, gridPosition.y].tileType);
            }
        }
        
        // !!!! Rotate action for building 
        
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     building[4].direction = BuildingData.GetNextDir(building[4].direction);
        // }
    }

    void InstantiateMap()
    {
        int emptyTileCount = GetEmptyTileCount();

        while (emptyTileCount > 0)
        {
            int batchSize = Mathf.Min(emptyTileCount, batchInstantiateSize);

            for (int i = 0; i < batchSize; i++)
            {
                Vector2Int gridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

                if (IsTileTypeEmpty(gridPosition.x, gridPosition.y))
                {
                    _grid._grid[gridPosition.x, gridPosition.y].tileType = GetRandomTileType();

                    _debugText[gridPosition.x, gridPosition.y].text = _grid._grid[gridPosition.x, gridPosition.y].tileType.ToString();
                    
                    GenerateMap(gridPosition.x, gridPosition.y);
                }
            }

            emptyTileCount = GetEmptyTileCount();
        }
    }

    void AroundTheBaseTile(int x, int z, TileType tileType, Color color)
    {
        if (_grid.IsInDimensions(new Vector2Int(x, z)) && _grid._grid[x,z].tileType == TileType.Empty)
        {
            _grid._grid[x,z].tileType = tileType;
            _debugText[x, z].text = _grid._grid[x, z].tileType.ToString();
            InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), tileType);
            _debugText[x, z].color = color;
        }
    }

    void TilePropagation (int x, int z, TileType tileType, Color color, int size, float noise)
    {
        for (int i = x - size; i <= x + size; i++)
        {
            for (int j = z - size; j <= z + size; j++)
            {
                if (Random.value < noise) 
                {
                    continue;
                }
                AroundTheBaseTile(i, j, tileType, color);
            }
        }
    }

    void GenerateMap(int x, int z)
    {
        TileType tileType = _grid._grid[x, z].tileType;
        
        switch (tileType)
        {
            case TileType.Grass:
                
                _debugText[x, z].color = Color.green;
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);


                TilePropagation(x, z, tileType, Color.green, Random.Range(1, grassPropagation), grassNoise);

                break;
            
            case TileType.Rock:
                
                _debugText[x, z].color = Color.yellow;
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Color.green, Random.Range(1, rockPropagation), rockNoise);

                break;
            
            case TileType.Water:
                
                _debugText[x, z].color = Color.blue;
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Color.green, Random.Range(3, waterPropagation), waterNoise);

                break;

            case TileType.Wood:

                _debugText[x, z].color = Color.red;
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Color.green, Random.Range(1, woodPropagation), woodNoise);

                break;

            case TileType.Iron:

                _debugText[x, z].color = Color.magenta;
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Color.green, Random.Range(1, ironPropagation), ironNoise);

                break;
        }
        
    }

    GameObject InstantiateTile(Vector3 position, TileType tileType)
    {
        GameObject tilePrefab = null;
        switch (tileType)
        {
            case TileType.Grass:
                tilePrefab = Instantiate(tilePrefabs[0], position, Quaternion.identity);
                break;
            
            case TileType.Rock:
                tilePrefab = Instantiate(tilePrefabs[1], position, quaternion.identity);
                break;  
            
            case TileType.Water:
                tilePrefab = Instantiate(tilePrefabs[2], position, quaternion.identity);
                break;
            
            case TileType.Wood:
                tilePrefab = Instantiate(tilePrefabs[3], position, quaternion.identity);
                break;
            
            case TileType.Iron:
                tilePrefab = Instantiate(tilePrefabs[4], position, quaternion.identity);
                break;
        }

        return tilePrefab;
    }

    int GetEmptyTileCount()
    {
        int count = 0;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if(_grid._grid[x,z].tileType == TileType.Empty)
                {
                    count++;
                }
            }
        }

        return count;
    }

    bool IsTileTypeEmpty(int x, int z)
    {
        return _grid._grid[x,z].tileType == TileType.Empty ? true : false;
    }

    TileType GetRandomTileType()
    {
        TileType tileType = TileType.Empty;
        int randomNumber = Random.Range(0, 100);

        if (randomNumber <= grassProbability)
        {
            tileType = TileType.Grass;
        }

        if (randomNumber > grassProbability && randomNumber <= rockProbability)
        {
            tileType = TileType.Rock;
        }

        if (randomNumber > rockProbability && randomNumber <= waterProbability)
        {
            tileType = TileType.Water;
        }

        if (randomNumber > waterProbability && randomNumber <= woodProbability)
        {
            tileType = TileType.Wood;
        }

        if (randomNumber > woodProbability && randomNumber <= ironProbability)
        {
            tileType = TileType.Iron;
        }

        return tileType;
    }
    
    private void DebugLines()
    {
        _debugText = new TextMesh[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                _debugText[x,z] = CreateWorldText(_grid._grid[x,z].tileType.ToString(), null, _grid.GridToWorldPosition(new Vector2Int(x, z)), 20, Color.white, TextAnchor.MiddleCenter);
                _debugText[x, z].color = Color.white;
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
