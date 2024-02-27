using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public Building building;
    public TileType tileType;
    public bool isFull;
};

public enum TileType{Empty, Rock, Wood, Iron, Water}
