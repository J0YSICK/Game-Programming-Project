using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 5f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference moveAction;

    private CharacterController _characterController;
    private Vector2 _moveInput;

    private void Awake(){
        _characterController = GetComponent<CharacterController>();

    }

    private void OnEnable(){
        moveAction.action.performed += StoreMovementInput;
        moveAction.action.canceled += StoreMovementInput;
    }

    private void StoreMovementInput(InputAction.CallbackContext context){
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update(){
        HandleMovement();
    }

    private void HandleMovement(){
        var move = cameraTransform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)).normalized;
        var currentSpeed = walkSpeed;
        var finalMove = move * currentSpeed;


        _characterController.Move(finalMove * Time.deltaTime);
    }
}