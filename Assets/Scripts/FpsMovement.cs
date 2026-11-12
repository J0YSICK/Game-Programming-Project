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
    [SerializeField] private InputActionReference crouchAction;
    [SerializeField] private InputActionReference sprintAction;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private bool _isRunning;
    private bool _isCrouching;
    private float _verticalVelocity;

    private void Awake(){
        _characterController = GetComponent<CharacterController>();

    }

    private void OnEnable(){
        moveAction.action.performed += StoreMovementInput;
        moveAction.action.canceled += StoreMovementInput;
        jumpAction.action.performed += Jump;
        sprintAction.action.performed += Sprint;
        sprintAction.action.canceled += Sprint;
        sprintAction.action.performed += Crouch;

    }

    private void OnDisable(){
        moveAction.action.performed -= StoreMovementInput;
        moveAction.action.canceled -= StoreMovementInput;
        jumpAction.action.performed -= Jump;    // should b negative
        sprintAction.action.performed -= Sprint;
        sprintAction.action.canceled -= Sprint;
        sprintAction.action.performed -= Crouch;
    }

    private void Crouch(InputAction.CallbackContext context){
        _isCrouching = !_isCrouching;
    }

    private void Sprint(InputAction.CallbackContext context){
        _isRunning = context.performed;
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
        var currentSpeed = _isCrouching ? crouchSpeed : _isRunning ? runSpeed : walkSpeed;
        var finalMove = move * currentSpeed;
        finalMove.y = _verticalVelocity;


        var collisions = _characterController.Move(finalMove * Time.deltaTime);
        if ((collisions & CollisionFlags.Above) != 0){
            _verticalVelocity = initialFallVelocity;
        }
    }
}