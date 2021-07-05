using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    [SerializeField] float minSwipeDistance = 0.2f;
    [SerializeField] float maxSwipeTime = 1f;

    private Vector2 startPos, endPos;
    private float startTime, endTime, incrementX, incrementY;
    private bool screenTapped = false;

    private void OnEnable()
    {
        input.OnStartTouch += SwipeStart;
        input.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        input.OnStartTouch -= SwipeStart;
        input.OnEndTouch += SwipeEnd;
    }

    private void SwipeStart(Vector2 coords, float time)
    {
        startPos = coords;
        startTime = time;
    }

    private void SwipeEnd(Vector2 coords, float time)
    {
        endPos = coords;
        endTime = time;
        Move();
    }

    private void Move()
    {
        if(Vector3.Distance(startPos, endPos) >= minSwipeDistance && (endTime - startTime) <= maxSwipeTime )
        {
            Debug.DrawLine(startPos, endPos, Color.red, 1f);

            incrementY = endPos.y - startPos.y;
            incrementX = endPos.x - startPos.x;
            screenTapped = false;

        } else
        {
            incrementX = endPos.x;
            incrementY = endPos.y;
            screenTapped = true;
        }

        if (Mathf.Abs(incrementX) > Mathf.Abs(incrementY) || screenTapped)
        {
            if (incrementX > 0)
            {
                Debug.Log("Attempting to rotate right!");

            }
            else
            {
                Debug.Log("Attempting to rotate left!");
            }
        }
        else
        {
            if (incrementY > 0)
            {
                Debug.Log("Attempting to go up!");

            }
            else
            {
                Debug.Log("Attempting to go down!");
            }
        }
    }
}
