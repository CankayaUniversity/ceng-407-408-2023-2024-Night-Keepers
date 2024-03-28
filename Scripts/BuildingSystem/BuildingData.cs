using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }
    
    public enum Dir
    {
        Up, Down, Left, Right
    }
    
    public enum BuildingType
    {
        Empty, StoneMine, IronMine, Lumberjack, TownHall, Test     
    }

    public BuildingType buildingTypes;
    public int health;
    public List<TileType> placableTileTypes;
    public Vector2Int widthHeight;
    
    public int wood;
    public int stone;
    public int iron;
    public int food;

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:  return 0;
            case Dir.Left:  return 90;
            case Dir.Up:    return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:  return new Vector2Int(0,0);
            case Dir.Left:  return new Vector2Int(0, widthHeight.y);
            case Dir.Up:    return new Vector2Int(widthHeight.y, 0);
            case Dir.Right: return new Vector2Int(0, 0);
        }
    }
    
    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < widthHeight.x; x++)
                {
                    for (int y = 0; y < widthHeight.y; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < widthHeight.y; x++)
                {
                    for (int y = 0; y < widthHeight.x; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }

        return gridPositionList;
    }
}
