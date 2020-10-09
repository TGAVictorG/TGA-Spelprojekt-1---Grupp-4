using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Roomba : MonoBehaviour
{

    public float myStartingMovementSpeed;
    public float myBackwardMovingTimeTarget = 2f;
    public float myRotationDegrees = 15f;
    public float myRotationTime = 3f;

    float myCurrentMovementSpeed;
    float myTimeMovingBackward;

    Rigidbody myRigidbody;

    public Vector3 targetRotationVector;

    int timesRotationHasChanged = 1;

    Quaternion myTargetRotation;

    public bool myShouldMovebackwards = false;
    
    bool myCanMove = true;
    bool shouldRotate = false;
    bool isRotating = false;
    private bool canChangeRotation = true;

    public bool isLookingForANewPath = false;
    private bool hasToggledMoveBackwards;
    private bool hasChangedRotation = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        ResetCurrentSpeed();

        targetRotationVector = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if(myCanMove)
        {
            switch (myShouldMovebackwards)
            {
                case true:
                    Move(-myCurrentMovementSpeed);
                    break;
                case false:
                    Move(myCurrentMovementSpeed);
                    break;
                default:
                    break;
            }
        }
       
        if(isLookingForANewPath)
        {
            LookForNewPath();
        }

        if(Input.GetMouseButtonDown(0))
        {
            targetRotationVector.y += myRotationDegrees;
        }
    }

    void LookForNewPath()
    {
        if(!hasToggledMoveBackwards)
        {
            ToggleMoveBackwards();
            hasToggledMoveBackwards = true;
            Debug.Log("Toggling move backwards");
        }

        if (myTimeMovingBackward < myBackwardMovingTimeTarget)
        {
            myTimeMovingBackward += Time.deltaTime;
        }
        else
        {
            myCanMove = false;
           // canChangeRotation = true;
            if(!hasChangedRotation)
            {
                ChangeTargetRotation();
                hasChangedRotation = true;
            }
            
            if (Mathf.Ceil(transform.rotation.eulerAngles.y) < targetRotationVector.y)
            {
                
                Debug.Log(Mathf.Ceil(transform.rotation.eulerAngles.y));
                Rotate();
            }
        }
    }

    void FoundNewPath()
    {
        
        Debug.Log("Found new path");
        ToggleMoveBackwards();
        hasToggledMoveBackwards = false;
        myCanMove = true;
        isLookingForANewPath = false;
        myTimeMovingBackward = 0;
        hasChangedRotation = false;
        canChangeRotation = true;
    }

    void ChangeTargetRotation()
    {
        if (canChangeRotation)
        {
           
            targetRotationVector.y += (myRotationDegrees+0.001f);

            if (targetRotationVector.y >= 360)
            {
                targetRotationVector.y = 0;
            }
            canChangeRotation = false;
        }
    }

    void Rotate()
    {
        transform.localEulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetRotationVector, myRotationTime * Time.deltaTime);

        if((Mathf.Ceil(transform.rotation.eulerAngles.y) >= targetRotationVector.y))
        {
            FoundNewPath();
        }
        
        

    }

    void ToggleMoveBackwards()
    {
        myShouldMovebackwards = !myShouldMovebackwards;
    }

    void Move(float aSpeed)
    {
        transform.position += transform.forward * aSpeed * Time.deltaTime;
    }

    void ResetCurrentSpeed()
    {
        myCurrentMovementSpeed = myStartingMovementSpeed / 100;
    }
}
