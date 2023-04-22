using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;

    public static Tile[,] graph;

    [SerializeField]
    private Vector3[] SpecialTiles; //z=tiletype

    [SerializeField] int mapSizeX = 10;
    [SerializeField] int mapSizeY = 10;

    //float oddRowXOffset = 0.86f;
    float zOffset = 1f;
    float xOffset = 1f;

    [SerializeField] Sprite StandardTile;
    [SerializeField] Sprite BlowerTile;
    [SerializeField] Sprite GrassTile;
    [SerializeField] Sprite LeafTile;
    [SerializeField] Sprite MowerTile;
    [SerializeField] Sprite ShovelTile;
    [SerializeField] Sprite SquirrelTile;

    void Start() {
        GeneratePathFindingGraph();
        GenerateMapVisual();
        //PlaceFiguresOnGraph();
    }

    public static Tile GetTileFromGrid(Vector2 position)
    {
        try
        {
            var tile = graph[(int)position.x, (int)position.y];
            return tile;
        }
        catch { return null; }
    }

    void GeneratePathFindingGraph() {
        graph = new Tile[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                graph[x, y] = new Tile();

                graph[x, y].trueX = (x * xOffset) + transform.position.x;
                graph[x, y].trueY = (y * zOffset) + transform.position.y;

                graph[x, y].gridX = x;
                graph[x, y].gridY = y;
                graph[x, y].tileType.Add(TileTypes.Standard);
            }
        }
        for (int x = 0; x < mapSizeX; x++) { //LEFT, RIGHT, DOWN, UP
            for (int y = 0; y < mapSizeY; y++) {
                if (x > 0) {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                } else {
                    graph[x, y].neighbours.Add(null);
                }
                if (x < mapSizeX - 1) {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                } else {
                    graph[x, y].neighbours.Add(null);
                }
                if (y > 0) {
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                } else {
                    graph[x, y].neighbours.Add(null);
                }
                if (y < mapSizeY - 1) {
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
                } else {
                    graph[x, y].neighbours.Add(null);
                }
            }
        }
        //set special tiles
        foreach (Vector3 tile in SpecialTiles) {
            TileTypes tileType = new TileTypes();
            switch (tile.z) {
                case 0:
                    tileType = TileTypes.Unaccessible;
                    break;
                case 1:
                    tileType = TileTypes.Tool;
                    break;
                case 2:
                    tileType = TileTypes.Grass;
                    break;
                case 3:
                    tileType = TileTypes.Leaves;
                    break;
                case 4:
                    tileType = TileTypes.Squirral;
                    break;
            }
            graph[(int)tile.x, (int)tile.y].tileType.Add(tileType);
            graph[(int)tile.x, (int)tile.y].tileType.Remove(TileTypes.Standard);
        }
        //set tools
        graph[0, 2].tool = Tools.Mower;
        graph[12, 2].tool = Tools.Mower;
        graph[0, 3].tool = Tools.LeafBlower;
        graph[12, 3].tool = Tools.LeafBlower;
        graph[0, 4].tool = Tools.Shovel;
        graph[12, 4].tool = Tools.Shovel;
    }

    void GenerateMapVisual() {
        foreach(Tile tile in graph) {
            GameObject hex_go = (GameObject)Instantiate(tilePrefab, new Vector3(tile.trueX, tile.trueY, 0), Quaternion.identity, transform);
            hex_go.name = $"Tile {tile.gridX},{tile.gridY}";
            tile.trueX = hex_go.transform.position.x;
            tile.trueY = hex_go.transform.position.y;
            //print($"Tile {tile.gridX},{tile.gridY} is at {tile.trueX},{tile.trueY}");
            tile.spriteRenderer = hex_go.GetComponent<SpriteRenderer>();
            switch (tile.tileType[0]) {
                case TileTypes.Unaccessible:
                    tile.spriteRenderer.color = Color.red;
                    break;
                case TileTypes.Standard:
                    tile.spriteRenderer.color = Color.white;
                    break;
                case TileTypes.Grass:
                    tile.spriteRenderer.color = Color.green;
                    tile.spriteRenderer.sprite = GrassTile;
                    break;
                case TileTypes.Leaves:
                    tile.spriteRenderer.color = Color.blue;
                    tile.spriteRenderer.sprite = LeafTile;
                    break;
                case TileTypes.Squirral:
                    tile.spriteRenderer.color = Color.yellow;
                    tile.spriteRenderer.sprite = SquirrelTile;
                    break;
                case TileTypes.Tool:
                    tile.spriteRenderer.color = Color.gray;
                    switch (tile.tool) {
                        case Tools.Mower:
                            tile.spriteRenderer.sprite = MowerTile;
                            break;
                        case Tools.LeafBlower:
                            tile.spriteRenderer.sprite = BlowerTile;
                            break;
                        case Tools.Shovel:
                            tile.spriteRenderer.sprite = ShovelTile;
                            break;
                    }
                    break;
            }
        }
    }

    private void ChangeTileSprite(int x, int y, Sprite s) {
        graph[x,y].spriteRenderer.sprite = s;
    }

    public void CutGrass(int x, int y) {
        ChangeTileSprite(x, y, StandardTile);
        graph[x, y].tileType[0] = TileTypes.Standard;
        graph[x, y].spriteRenderer.color = Color.white;
    } 
}
