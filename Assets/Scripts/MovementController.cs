using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    [SerializeField] private float minSwipeDistance = 0.2f;
    [SerializeField] private float maxSwipeTime = 1f;
    [SerializeField] private float rotationTime = 0.5f, heightChangeTime = 0.25f;

    [SerializeField] private Platform[] platformsInterior, platformsExterior;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform lowerPos, higherPos;

    private Platform[][] platforms;
    private int currentPlatformIndex = 0, currentHeight = 0;
    private Vector2 startPos, endPos;
    private float startTime, endTime, incrementX, incrementY, rotationAngle = 0, rotationSpeed;
    private bool isMoving = false;

    public delegate void RestartLevelEvent();
    public event RestartLevelEvent RestartLevel;
    public delegate void NextLevelEvent();
    public event NextLevelEvent NextLevel;

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
        rotationSpeed = 1 / rotationTime;

        platforms = new Platform[2][];
        platforms[0] = platformsInterior;
        platforms[1] = platformsExterior;

        setCurrentRotatingAngle();
    }

    private void setCurrentRotatingAngle()
    {
        this.rotationAngle = 360.0f / platforms[currentHeight].Length;
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
        if(!isMoving) Move();
    }

    private void Move()
    {
        if(Vector3.Distance(startPos, endPos) >= minSwipeDistance && (endTime - startTime) <= maxSwipeTime )
        {
            incrementX = endPos.x - startPos.x;
            incrementY = endPos.y - startPos.y;

        } else
        {
            incrementX = endPos.x * -1;
            incrementY = 0;
        }

        if (Mathf.Abs(incrementX) > Mathf.Abs(incrementY))
        {
            if (incrementX > 0)
            {
                // Rotating to the right

                if (canRotate(true))
                {
                    StartCoroutine(Rotate(true));

                }
            }
            else
            {
                // Rotating to the left

                if (canRotate(false))
                {
                    StartCoroutine(Rotate(false));

                }
            }
        }
        else
        {
            if (incrementY > 0)
            {
                // Going up

                if(canChangeHeight(true))
                {
                    StartCoroutine(changeHeight(true));
                }
            }
            else
            {
                // Going down

                if (canChangeHeight(false))
                {
                    StartCoroutine(changeHeight(false));
                }
            }
        }
    }

    IEnumerator Rotate(bool rotatingRight)
    {
        isMoving = true;

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        setCurrentRotatingAngle();

        float t = 0.0f;

        while (t < rotationTime)
        {
            t += Time.deltaTime;

            if (!rotatingRight)
            {
                transform.Rotate(0.0f, 0.0f, rotationAngle * Time.deltaTime * rotationSpeed);
            } else
            {
                transform.Rotate(0.0f, 0.0f, rotationAngle * Time.deltaTime * rotationSpeed * -1);
            }

            yield return null;
        }

        if (!rotatingRight) // Rotation ends
        {
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z + rotationAngle);
        } else
        {
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z - rotationAngle);
        }

        isMoving = false;

        damageCurrentPlatform();

    }

    IEnumerator changeHeight(bool goingUp)
    {
        isMoving = true;

        Vector3 currentPos, finalPos;
        
        if(goingUp)
        {
            currentPos = lowerPos.position;
            finalPos = higherPos.position;
            currentHeight = 1;

        } else
        {
            currentPos = higherPos.position;
            finalPos = lowerPos.position;
            currentHeight = 0;
        }

        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / heightChangeTime;
            player.transform.position = Vector3.Lerp(currentPos, finalPos, t);
            yield return null;
        }

        isMoving = false;

        damageCurrentPlatform();

    }

    private bool canRotate(bool rotatingRight)
    {
        int previousPosition = currentPlatformIndex;

        if (rotatingRight)
        {
            currentPlatformIndex++;

            if (currentPlatformIndex > platforms[currentHeight].Length -1) currentPlatformIndex = 0;

        } else
        {
            currentPlatformIndex--;

            if (currentPlatformIndex < 0) currentPlatformIndex = platforms[currentHeight].Length -1;
        }

        if (!platforms[currentHeight][currentPlatformIndex].IsWalkable())
        {
            currentPlatformIndex = previousPosition;
            return false;
        }

        return true;
    }

    private bool canChangeHeight(bool goingUp)
    {
        if ((currentHeight == 1 && goingUp) || (currentHeight == 0 && !goingUp) || platforms[1].Length == 0 || platforms[0].Length == 0 || platforms[currentHeight][currentPlatformIndex].getCanChangeHeight() == false) return false;

        if((goingUp && !platforms[1][currentPlatformIndex].IsWalkable()) || (!goingUp && !platforms[0][currentPlatformIndex].IsWalkable()))
        {
            return false;
        } 

        return true;
    }

    private void damageCurrentPlatform()
    {
        if (platforms[currentHeight][currentPlatformIndex].isBroken()) 
        {
            if (RestartLevel != null) RestartLevel();

            isMoving = true;
        }

        platforms[currentHeight][currentPlatformIndex].reduceDurability();

        foreach(Platform[] plats in platforms) {
            foreach(Platform p in plats)
            {
                if (!p.isBroken()) return;
            }
        }

        if (NextLevel != null) NextLevel();
    }
}
