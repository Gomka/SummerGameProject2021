using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private InputHandler input;
    private Camera cameraMain;
    private void Awake()
    {
        cameraMain = Camera.main;   
    }

    private void OnEnable()
    {
        input.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        input.OnStartTouch -= Move;
    }

    private void Move(Vector2 coords)
    {
        Vector3 screenCoordinates = new Vector3(coords.x, coords.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;

        if (worldCoordinates.y > 2.5)
        {
            Debug.Log("Attempting to go up! " + worldCoordinates);

        }
        else if (worldCoordinates.y < -2.5)
        {
            Debug.Log("Attempting to go down! " + worldCoordinates);
        }
        else if (worldCoordinates.x > 0)
        {
            Debug.Log("Attempting to rotate right! " + worldCoordinates);

        }
        else if (worldCoordinates.x < 0)
        {
            Debug.Log("Attempting to rotate left! " + worldCoordinates);

        }
    }
}
