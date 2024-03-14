using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public Building building;
    public TileType tileType;
    public Texture2D tileTexture;
    public bool isFull;
};

public enum TileType{Empty, Grass ,Rock, Wood, Iron, Water}
