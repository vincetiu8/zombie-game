using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    
    private Vector2 movementDirection;
    
    private void Update()
    {
        transform.position += (Vector3) movementDirection * movementSpeed;
    }

    public void UpdateMovementDirection(InputAction.CallbackContext context)
    {
        Debug.Log(movementDirection);
        movementDirection = context.ReadValue<Vector2>();
    }
}
