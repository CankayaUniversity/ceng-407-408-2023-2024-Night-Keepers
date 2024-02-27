using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    private int _width;
    private int _height;
    private int _cellsize;
    public T [,] _grid;
    private TextMesh[,] _debugText;

    public T this[Vector2Int gridPosition]
    {
        get => _grid[gridPosition.x, gridPosition.y];
        set => _grid[gridPosition.x, gridPosition.y] = value;
    }
    
    public Grid(int width, int height, int cellsize)
    {
        _width = width;
        _height = height;
        _cellsize = cellsize;
        _grid = new T[_width, _height];
        
    }
    
    public void SetItem(Vector2Int gridPosition, T item)
    {
        _grid[gridPosition.x, gridPosition.y] = item;
        
    }

    public void SetItem(Vector3 worldPosition, T item)
    {
        Vector2Int gridPosition = WorldToGridPosition(worldPosition);
        SetItem(gridPosition, item);
    }

    public T GetItem(Vector2Int gridPosition)
    {
        return _grid[gridPosition.x, gridPosition.y];
    }

    public T GetItem(Vector3 worldPosition)
    {
        Vector2Int gridPosition = WorldToGridPosition(worldPosition);
        return GetItem(gridPosition);
    }
    
    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y) * _cellsize + new Vector3(1, 0, 1) * (_cellsize * 0.5f);
    }
    
    public Vector3 GridToWorldPositionDrawLine(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y) * _cellsize;
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / _cellsize);
        int z = Mathf.FloorToInt(worldPosition.z / _cellsize);

        return new Vector2Int(x, z);
    }

    public bool IsInDimensions(Vector2Int gridPosition)
    {
        return (gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < _grid.GetLength(0) && gridPosition.y < _grid.GetLength(1));
    }
}
