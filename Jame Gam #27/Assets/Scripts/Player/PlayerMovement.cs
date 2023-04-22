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
        //if input is triggered
        if(_direction != Vector2.zero)
        {
            //check if tile exists
            var tile = DoesTileExist(_direction);
            print(tile);
            if (tile == null || tile.tileType.Contains(TileTypes.Unaccessible)) return;
            else
            {
                //if player is not already moving, move them and update their current tile
                _playerAnimator.SetAnimation(_direction);
                if (!_isMoving) await MovePlayer(_direction);
                _currentTile = tile;
            }
        }
    }

    private async Task MovePlayer(Vector2 direction)
    {
        _isMoving = true;

        float elapsedTime = 0;
        _originPos = transform.position;
        _targetPos = _originPos + direction;

        //increment elapsed time while moving player until time is reached
        while (elapsedTime < _timeToMove)
        {
            transform.position = Vector2.Lerp(_originPos, _targetPos, (elapsedTime / _timeToMove));
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }

        //ensure that the player lands exactly on a tile's center
        transform.position = _targetPos;

        _isMoving = false;
        _playerAnimator.SetAnimation(Vector2.zero);
    }

    //Check whether or not the tile the player is moving to is null
    private Tile DoesTileExist(Vector2 input)
    {
        var nextTileX = (int)(_currentTile.gridX + input.normalized.x);
        int nextTileY = (int)(_currentTile.gridY + input.normalized.y);
        return GridManager.GetTileFromGrid(new Vector2(nextTileX,nextTileY));
    }
}
