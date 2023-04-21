using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes {Unaccessible, Standard, Grass, Leaves, Squirral, Tool}

public class Tile
{
    public List<TileTypes> tileType = new List<TileTypes>();
    public List<Tile> neighbours = new List<Tile>();
    public int gridX;
    public int gridY;
    public float trueX;
    public float trueY;

}