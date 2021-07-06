using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputHandler : MonoBehaviour
{
    public InputMaster input;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private Camera cameraMain;

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
        cameraMain = Camera.main;

        // Subscribing to the input functions
        input = new InputMaster();
        input.Player.TouchPress.started += ctx => StartTouch(ctx);
        input.Player.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) OnStartTouch(ScreenToWorld(input.Player.Move.ReadValue<Vector2>()), (float) context.startTime);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) OnEndTouch(ScreenToWorld(input.Player.Move.ReadValue<Vector2>()), (float) context.time);
    }

    public Vector2 FingerPosition()
    {
        return ScreenToWorld(input.Player.Move.ReadValue<Vector2>());
    }

    private Vector2 ScreenToWorld(Vector2 screen)
    {
        Vector3 screenCoordinates = new Vector3(screen.x, screen.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;

        return worldCoordinates;
    }
}
