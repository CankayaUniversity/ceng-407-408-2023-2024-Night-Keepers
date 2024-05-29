using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public int cellSize;

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
    [SerializeField] private List<GameObject> tilePrefabs;
    [SerializeField] private Transform waterParent;

    public static event Action onWorldGenerationDone;
    public Grid<Tile> _grid;
    private const int batchInstantiateSize = 1000;
    private void Awake()
    {
        _grid = new Grid<Tile>(width, height, cellSize);
        InstantiateMap();
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
                    GenerateMap(gridPosition.x, gridPosition.y);
                }
            }

            emptyTileCount = GetEmptyTileCount();
        }
        onWorldGenerationDone?.Invoke();
    }

    void AroundTheBaseTile(int x, int z, TileType tileType)
    {
        if (_grid.IsInDimensions(new Vector2Int(x, z)) && _grid._grid[x,z].tileType == TileType.Empty)
        {
            _grid._grid[x,z].tileType = tileType;
            InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), tileType);
        }
    }

    void TilePropagation (int x, int z, TileType tileType, int size, float noise)
    {
        for (int i = x - size; i <= x + size; i++)
        {
            for (int j = z - size; j <= z + size; j++)
            {
                if (Random.value < noise) 
                {
                    continue;
                }
                AroundTheBaseTile(i, j, tileType);
            }
        }
    }

    void GenerateMap(int x, int z)
    {
        TileType tileType = _grid._grid[x, z].tileType;
        
        switch (tileType)
        {
            case TileType.Grass:                
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Random.Range(1, grassPropagation), grassNoise);

                break;
            
            case TileType.Rock:
                
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Random.Range(1, rockPropagation), rockNoise);

                break;
            
            case TileType.Water:
                
                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Random.Range(3, waterPropagation), waterNoise);

                break;

            case TileType.Wood:

                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Random.Range(1, woodPropagation), woodNoise);

                break;

            case TileType.Iron:

                InstantiateTile(_grid.GridToWorldPosition(new Vector2Int(x, z)), _grid._grid[x, z].tileType);

                TilePropagation(x, z, tileType, Random.Range(1, ironPropagation), ironNoise);

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
                GameObject waterObj = new GameObject();
                tilePrefab = Instantiate(waterObj, position, quaternion.identity, waterParent);
                // tilePrefab = Instantiate(tilePrefabs[2], position, quaternion.identity, waterParent);
                break;
            
            case TileType.Wood:
                tilePrefab = Instantiate(tilePrefabs[3], position, quaternion.identity);
                break;
            
            case TileType.Iron:
                tilePrefab = Instantiate(tilePrefabs[4], position, quaternion.identity);
                break;
        }
        
        tilePrefab.transform.localScale = new Vector3((float)cellSize / 10, (float)cellSize / 10, (float)cellSize / 10);

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

    public int GetMapSizeFromCenter()
    {
        return width * cellSize / 2;
    }
}
