using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerAnimator _playerAnimator;
    private Rigidbody2D _rb;
    private InputAction _moveAction;
    private Vector2 _input;
    public float speed = 5;
    

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _rb = GetComponent<Rigidbody2D>();
        _moveAction = _playerInput.actions["Move"];
    }

    void Update()
    {
        _input = _moveAction.ReadValue<Vector2>();
        print(_input.magnitude);
        _playerAnimator.SetAnimation(_input);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _input * speed * Time.fixedDeltaTime);
    }
}
