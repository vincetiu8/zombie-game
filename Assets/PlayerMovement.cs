using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 _movementDirection;

    private void Update()
    {
        transform.position += (Vector3)_movementDirection * movementSpeed;
    }

    public void UpdateMovementDirection(InputAction.CallbackContext context)
    {
        Debug.Log(_movementDirection);
        _movementDirection = context.ReadValue<Vector2>();
    }
}