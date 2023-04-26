using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes {Unaccessible, Standard, Grass, Tool}
public enum Tools {None,Mower,LeafBlower,Shovel}

public class Tile
{
    public TileTypes tileType = TileTypes.Standard;
    public List<Tile> neighbours = new List<Tile>(); //LEFT, RIGHT, DOWN, UP
    public int gridX;
    public int gridY;
    public float trueX;
    public float trueY;
    //public SpriteRenderer spriteRenderer;
    public Tools tool;
    public int leaves = 0;
    public TileGraphics tileGraphics;
    public bool hasSquirrel = false;
}
