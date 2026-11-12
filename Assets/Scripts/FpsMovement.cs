using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 5f;

    [Header("Jump and Fall")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -12f;
    // the -2 below prevents weird jumps when going donw the stairs
    [SerializeField] private float initialFallVelocity = -2f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private float _verticalVelocity;

    private void Awake(){
        _characterController = GetComponent<CharacterController>();

    }

    private void OnEnable(){
        moveAction.action.performed += StoreMovementInput;
        moveAction.action.canceled += StoreMovementInput;
        jumpAction.action.performed += Jump;
    }

    private void OnDisable(){
        moveAction.action.performed -= StoreMovementInput;
        moveAction.action.canceled -= StoreMovementInput;
        jumpAction.action.performed -= Jump;    // should b negative
    }

    private void StoreMovementInput(InputAction.CallbackContext context){
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update(){
        _isGrounded = _characterController.isGrounded;
        HandleGravity();
        HandleMovement();
    }

    private void Jump (InputAction.CallbackContext context){
        if (_isGrounded) {
            _verticalVelocity = jumpForce;
        }
    }

    private void HandleGravity(){
        if (_isGrounded && _verticalVelocity < 0){
            _verticalVelocity = initialFallVelocity;
        }

        _verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleMovement(){
        var move = cameraTransform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)).normalized;
        var currentSpeed = walkSpeed;
        var finalMove = move * currentSpeed;
        finalMove.y = _verticalVelocity;


        _characterController.Move(finalMove * Time.deltaTime);
    }
}