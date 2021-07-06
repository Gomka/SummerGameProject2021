using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    [SerializeField] private float minSwipeDistance = 0.2f;
    [SerializeField] private float maxSwipeTime = 1f;
    [SerializeField] private float rotationTime = 0.5f;
    [SerializeField] private int numSegments = 4;

    private Vector2 startPos, endPos;
    private float startTime, endTime, incrementX, incrementY, rotationAngle;
    private bool isMoving = false, screenTapped = false;

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

    private void Start()
    {
        rotationAngle = 360.0f / numSegments;
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
                if (!isMoving) StartCoroutine(Rotate(true));
            }
            else
            {
                if (!isMoving) StartCoroutine(Rotate(false));
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

    IEnumerator Rotate(bool rotatingRight)
    {
        isMoving = true;

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        float t = 0.0f;

        while (t < rotationTime)
        {
            t += Time.deltaTime;
            if (!rotatingRight)
            {
                transform.Rotate(0.0f, 0.0f, rotationAngle * Time.deltaTime * 2);
            } else
            {
                transform.Rotate(0.0f, 0.0f, rotationAngle * Time.deltaTime * -2);
            }

            yield return null;
        }

        if (!rotatingRight)
        {
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z + rotationAngle);
        } else
        {
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z - rotationAngle);
        }

        if (t >= rotationTime) isMoving = false;

    }
    
}
