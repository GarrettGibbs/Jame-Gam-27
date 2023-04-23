using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;

    public static Tile[,] graph;
    private List<Tile> player1Tiles = new List<Tile>();
    private List<Tile> player2Tiles = new List<Tile>();

    [SerializeField]
    private Vector3[] SpecialTiles; //z=tiletype

    [SerializeField] int mapSizeX = 10;
    [SerializeField] int mapSizeY = 10;

    float zOffset = 1f;
    float xOffset = 1f;

    //[SerializeField] Sprite StandardTile;
    [SerializeField] GameObject[] Player1Tools;
    [SerializeField] GameObject[] Player2Tools;

    int zIndex = 0;

    void Start() {
        GeneratePathFindingGraph();
        GenerateMapVisual();
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
                graph[x, y].tileType = TileTypes.Standard;
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
        for (int x = 0; x < mapSizeX; x++) { //populate player specific graphs
            for (int y = 0; y < mapSizeY; y++) {
                if (x > 0 && x < 6) player1Tiles.Add(graph[x, y]);
                else if (x > 6 && x < 12) player2Tiles.Add(graph[x, y]);
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
                //case 2:
                //    tileType = TileTypes.Grass;
                //    break;
                //case 3:
                //    tileType = TileTypes.Standard;
                //    graph[(int)tile.x, (int)tile.y].leaves++;
                //    break;
                //case 4:
                //    tileType = TileTypes.Squirral;
                //    break;
            }
            graph[(int)tile.x, (int)tile.y].tileType = tileType;
        }
        //set tools
        graph[0, 2].tool = Tools.Mower;
        graph[12, 2].tool = Tools.Mower;
        graph[0, 3].tool = Tools.LeafBlower;
        graph[12, 3].tool = Tools.LeafBlower;
        graph[0, 4].tool = Tools.Shovel;
        graph[12, 4].tool = Tools.Shovel;

        RandomizeStart(player1Tiles);
        RandomizeStart(player2Tiles);
    }

    private void RandomizeStart(List<Tile> side) {
        List<Tile> tempTiles = side;
        //Squrriels
        Tile rand1 = tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)];
        rand1.tileType = TileTypes.Squirral;
        tempTiles.Remove(rand1);
        Tile rand2 = tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)];
        rand2.tileType = TileTypes.Squirral;
        tempTiles.Remove(rand2);
        //leaves (currently allows for stacking)
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)].leaves++;
        foreach(Tile tile in tempTiles) {
            tile.tileType = TileTypes.Grass;
        }
    }

    void GenerateMapVisual() {
        foreach(Tile tile in graph) {
            GameObject hex_go = (GameObject)Instantiate(tilePrefab, new Vector3(tile.trueX, tile.trueY, 0), Quaternion.identity, transform);
            hex_go.name = $"Tile {tile.gridX},{tile.gridY}";
            tile.trueX = hex_go.transform.position.x;
            tile.trueY = hex_go.transform.position.y;
            hex_go.transform.position = new Vector3(hex_go.transform.position.x, hex_go.transform.position.y, zIndex);
            zIndex++;
            tile.tileGraphics = hex_go.GetComponent<TileGraphics>();
            switch (tile.tileType) {
                case TileTypes.Unaccessible:
                    hex_go.SetActive(false);
                    break;
                case TileTypes.Grass:
                    tile.tileGraphics.GrassImage.SetActive(true);
                    break;
                case TileTypes.Squirral:
                    tile.tileGraphics.SquirrelHoleImage.SetActive(true);
                    tile.tileGraphics.SquirrelImage.SetActive(true);
                    break;
                case TileTypes.Tool:
                    hex_go.SetActive(false);
                    break;
            }
            if(tile.leaves > 0) {
                ShowLeaves(tile, tile.leaves);
            }
        }
    }

    public void CutGrass(int x, int y) {
        graph[x, y].tileType = TileTypes.Standard;
        graph[x, y].tileGraphics.GrassImage.SetActive(false);
    }

    public void UpdateTools(Tools tool, int player) {
        GameObject[] tempTools;
        if (player == 1) tempTools = Player1Tools;
        else tempTools = Player2Tools;
        switch (tool) {
            case Tools.Mower:
                tempTools[0].SetActive(false);
                tempTools[1].SetActive(true);
                tempTools[2].SetActive(true);
                break;
            case Tools.LeafBlower:
                tempTools[0].SetActive(true);
                tempTools[1].SetActive(false);
                tempTools[2].SetActive(true);
                break;
            case Tools.Shovel:
                tempTools[0].SetActive(true);
                tempTools[1].SetActive(true);
                tempTools[2].SetActive(false);
                break;
        }
    }

    public void BlowLeaves(Vector2 direction, Tile tile, int player) {
        Tile targetTile = null;
        if (direction.y > .01) targetTile = tile.neighbours[3]; //UP
        else if (direction.y < -.01) targetTile = tile.neighbours[2]; //DOWN
        else if (direction.x > .01) targetTile = tile.neighbours[1]; //RIGHT
        else if (direction.x < -.01) targetTile = tile.neighbours[0]; //LEFT
        if (targetTile == null) { //off the side
            tile.leaves = 0;
            TurnOffLeaves(tile);
            return;
        }
        if (targetTile.tileType == TileTypes.Squirral || targetTile.tileType == TileTypes.Tool) return;

        if(targetTile.gridX == 6) { //accross the middle
            SprayLeaves(tile.leaves, player, tile);
            tile.leaves = 0;
            TurnOffLeaves(tile);
        } else { //normal moving leaves
            targetTile.leaves = Mathf.Min(3, targetTile.leaves + tile.leaves);
            tile.leaves = 0;
            TurnOffLeaves(tile);
            ShowLeaves(targetTile, targetTile.leaves);
        }
    }

    private void TurnOffLeaves(Tile tile) {
        tile.tileGraphics.Leaves1Image.SetActive(false);
        tile.tileGraphics.Leaves2Image.SetActive(false);
        tile.tileGraphics.Leaves3Image.SetActive(false);
    }

    private void ShowLeaves(Tile tile, int amount) {
        if (amount == 1) {
            tile.tileGraphics.Leaves1Image.SetActive(true);
            tile.tileGraphics.Leaves2Image.SetActive(false);
            tile.tileGraphics.Leaves3Image.SetActive(false);
        } else if(amount == 2) {
            tile.tileGraphics.Leaves1Image.SetActive(false);
            tile.tileGraphics.Leaves2Image.SetActive(true);
            tile.tileGraphics.Leaves3Image.SetActive(false);
        } else {
            tile.tileGraphics.Leaves1Image.SetActive(false);
            tile.tileGraphics.Leaves2Image.SetActive(false);
            tile.tileGraphics.Leaves3Image.SetActive(true);
        }
    }

    private void SprayLeaves(int amount, int player, Tile tile) {
        List<Tile> tempTiles = new List<Tile>();
        if (player == 1) {
            foreach (Tile spot in player2Tiles) {
                if (spot.tileType != TileTypes.Squirral && spot.leaves == 0) tempTiles.Add(spot);
            }
        } else {
            foreach (Tile spot in player1Tiles) {
                if (spot.tileType != TileTypes.Squirral && spot.leaves == 0) tempTiles.Add(spot);
            }
        } 

        if(amount == 1) {
            List<Tile> tempTiles2 = new List<Tile>();
            foreach(Tile spot in tempTiles) if(spot.gridY == tile.gridY) tempTiles2.Add(spot);
            Tile newSpot = tempTiles2[UnityEngine.Random.Range(0, tempTiles2.Count)];
            newSpot.leaves++;
            ShowLeaves(newSpot, newSpot.leaves);
        } else {
            for (int i = 0; i < amount; i++) {
                Tile newSpot = tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)];
                newSpot.leaves++;
                ShowLeaves(newSpot, newSpot.leaves);
                tempTiles.Remove(newSpot);
            }
        }
    }

    public async void HitSquirrel(Tile tile, int player) {
        if(tile.tileType != TileTypes.Squirral) return;
        tile.tileGraphics.SquirrelHoleImage.SetActive(false);
        tile.tileGraphics.SquirrelImage.SetActive(false);
        tile.tileType = TileTypes.Standard;

        List<Tile> tempTiles = new List<Tile>();
        if (player == 1) {
            foreach (Tile spot in player2Tiles) {
                if (spot.tileType == TileTypes.Standard && spot.leaves == 0) tempTiles.Add(spot);
            }
        } else {
            foreach (Tile spot in player1Tiles) {
                if (spot.tileType == TileTypes.Standard && spot.leaves == 0) tempTiles.Add(spot);
            }
        }
        if(tempTiles.Count == 0) return;
        Tile newSpot = tempTiles[UnityEngine.Random.Range(0, tempTiles.Count)];
        newSpot.tileGraphics.SquirrelHoleImage.SetActive(true);
        await Task.Delay(750);
        newSpot.tileGraphics.SquirrelImage.SetActive(true);
        newSpot.tileType = TileTypes.Squirral;
    }
}
