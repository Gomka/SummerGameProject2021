using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public InputMaster input;

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Awake()
    {
        input = new InputMaster();
        input.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    public void Move(Vector2 direction)
    {
        if (direction.x > 0)
        {
            Debug.Log("Attempting to rotate right!");

        } else if (direction.x < 0)
        {
            Debug.Log("Attempting to rotate left!");

        } else if (direction.y > 0)
        {
            Debug.Log("Attempting to go up!");

        } else if (direction.y < 0)
        {
            Debug.Log("Attempting to go down!");
        }
    }
}
