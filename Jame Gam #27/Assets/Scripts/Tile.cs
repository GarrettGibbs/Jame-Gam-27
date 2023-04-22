using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes {Unaccessible, Standard, Grass, Leaves, Squirral, Tool}
public enum Tools {None,Mower,LeafBlower,Shovel}

public class Tile
{
    public List<TileTypes> tileType = new List<TileTypes>();
    public List<Tile> neighbours = new List<Tile>(); //LEFT, RIGHT, DOWN, UP
    public int gridX;
    public int gridY;
    public float trueX;
    public float trueY;
    public SpriteRenderer spriteRenderer;
    public Tools tool;
}
