using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor.Rendering;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //animations
    private PlayerAnimator _playerAnimator;

    //input 
    private PlayerInput _playerInput;
    private InputAction _upAction;
    private InputAction _downAction;
    private InputAction _leftAction;
    private InputAction _rightAction;

    //position
    [SerializeField] private Vector2 _startingLocation;
    private Tile _currentTile;
    private float _pivotOffsetY = .65f;

    //movement
    private Vector2 _direction;
    private bool _isMoving;
    private Vector2 _originPos, _targetPos;
    private float _timeToMove = .2f;

    //equipment
    private Tools equippedTool = Tools.None;
    [SerializeField] GridManager gridManager;
    private bool _isActing;
    [SerializeField] int PlayerNumber;
    [SerializeField] ToolGraphics _toolGraphics;
    [SerializeField] GameManager gameManager;

    //score
    [SerializeField] private ScoreBar _scoreBar;

    async void Awake()
    {
        _isMoving = false;
        _playerAnimator = GetComponent<PlayerAnimator>();

        _playerInput = GetComponent<PlayerInput>();
        _upAction = _playerInput.actions["Up"];
        _downAction = _playerInput.actions["Down"];
        _leftAction = _playerInput.actions["Left"];
        _rightAction = _playerInput.actions["Right"];

        //wait one secend to ensure grid has loaded in
        await Task.Delay(1000);

        //get starting tile, and then set player transform to that tile
        _currentTile = GridManager.graph[(int)_startingLocation.x, (int)_startingLocation.y];
        transform.position = new Vector2(_currentTile.trueX, _currentTile.trueY + _pivotOffsetY);
    }

    void Update()
    {
        //check if any direction key is pressed, and set direction value
        if (_upAction.IsPressed()) _direction = Vector2.up;
        else if (_downAction.IsPressed()) _direction = Vector2.down;
        else if (_leftAction.IsPressed()) _direction = Vector2.left;
        else if (_rightAction.IsPressed()) _direction = Vector2.right;
        else _direction = Vector2.zero;
    }

    private async void FixedUpdate()
    {
        if(!gameManager.inGame) return;
        //if input is triggered
        if(_direction != Vector2.zero)
        {
            //check if tile exists
            var tile = DoesTileExist(_direction);
            //print(tile);
            if (_isMoving || _isActing) return;
            _playerAnimator.TurnTowards(_direction);
            _toolGraphics.ChangeToolGraphic(equippedTool, _direction);
            if (tile == null || tile.tileType == TileTypes.Unaccessible || (tile.hasSquirrel && equippedTool != Tools.Shovel)) return;
            else if(tile.leaves > 0) {
                if (equippedTool == Tools.LeafBlower) {
                    await BlowLeaves(_direction, tile);
                } else {
                    await MovePlayer(_direction, tile);
                }
            } else if (tile.hasSquirrel) {
                if (equippedTool == Tools.Shovel) {
                    await HitSquirrel(_direction, tile);
                }
            } else {
                switch (tile.tileType) {
                    case TileTypes.Tool:
                        await MovePlayer(_direction, tile);
                        if (equippedTool == tile.tool) return;
                        gameManager.audioManager.PlaySound($"Shovel_Pickup_{Random.Range(1, 3)}");
                        equippedTool = tile.tool;
                        if (equippedTool == Tools.Mower) gameManager.audioManager.StartMowerIdle(PlayerNumber);
                        else gameManager.audioManager.StopMowerIdle(PlayerNumber);
                        gridManager.UpdateTools(equippedTool, PlayerNumber);
                        _toolGraphics.ChangeToolGraphic(equippedTool, _direction);
                        break;
                    case TileTypes.Grass:
                        if (equippedTool == Tools.Mower && tile.leaves == 0) {
                            await MowGrass(_direction, tile);
                        } else {
                            await MovePlayer(_direction, tile);
                        }
                        break;
                    default:
                        await MovePlayer(_direction, tile);
                        break;
                }
                    
            }
        }
    }

    private async Task MovePlayer(Vector2 direction, Tile tile) {
        _isMoving = true;
        gameManager.audioManager.PlaySound("Walk grass");
        _playerAnimator.SetAnimation(direction);

        _originPos = transform.position;
        _targetPos = _originPos + direction;


        LeanTween.move(gameObject, _targetPos, _timeToMove);
        await Task.Delay(200);
        //ensure that the player lands exactly on a tile's center
        transform.position = _targetPos;

        _currentTile = tile;
        _isMoving = false;
        _playerAnimator.SetAnimation(Vector2.zero);
        await Task.Yield();
    }

    private async Task MowGrass(Vector2 direction, Tile tile) {
        _isActing = true;
        gameManager.audioManager.PlayMowerAction(PlayerNumber);
        tile.tileGraphics.ShowGrassPS();
        await Task.Delay(1000);
        gridManager.CutGrass(tile.gridX, tile.gridY);
        //SCORE POINTS
        if (PlayerNumber == 1) _scoreBar.UpdateLeft();
        else if (PlayerNumber == 2) _scoreBar.UpdateRight();
        _isActing = false;
        await Task.Yield();
    }

    private async Task BlowLeaves(Vector2 direction, Tile tile) {
        _isActing = true;
        tile.tileGraphics.ShowLeavesPS();
        gameManager.audioManager.PlaySound("LeafBlower");
        await Task.Delay(1000);
        var leavesRemoved = gridManager.BlowLeaves(direction, tile, PlayerNumber);
        //SCORE POINTS
        if (leavesRemoved && PlayerNumber == 1) _scoreBar.UpdateLeft();
        else if (leavesRemoved && PlayerNumber == 2) _scoreBar.UpdateRight();
        _isActing = false;
        await Task.Yield();
    }

    private async Task HitSquirrel(Vector2 direction, Tile tile) {
        tile.tileGraphics.ShowDustPS();
        _isActing = true;
        await Task.Delay(1000);
        gridManager.HitSquirrel(tile, PlayerNumber);
        _isActing = false;
        await Task.Yield();
    }

    //private async Task MovePlayer(Vector2 direction)
    //{
    //    _isMoving = true;

    //    float elapsedTime = 0;
    //    _originPos = transform.position;
    //    _targetPos = _originPos + direction;

    //    //increment elapsed time while moving player until time is reached
    //    while (elapsedTime < _timeToMove)
    //    {
    //        transform.position = Vector2.Lerp(_originPos, _targetPos, (elapsedTime / _timeToMove));
    //        elapsedTime += Time.deltaTime;
    //        await Task.Yield();
    //    }

    //    //ensure that the player lands exactly on a tile's center
    //    transform.position = _targetPos;

    //    _isMoving = false;
    //    _playerAnimator.SetAnimation(Vector2.zero);
    //}

    //Check whether or not the tile the player is moving to is null
    private Tile DoesTileExist(Vector2 input)
    {
        var nextTileX = (int)(_currentTile.gridX + input.normalized.x);
        int nextTileY = (int)(_currentTile.gridY + input.normalized.y);
        return GridManager.GetTileFromGrid(new Vector2(nextTileX,nextTileY));
    }
}
