using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Roomba : MonoBehaviour
{
    [Header("Movement settings")]
    [Tooltip("The forward speed of the roomba")]
    public float myStartingMovementSpeed = 100f;

    [Tooltip("The reversing speed of the roomba")]
    public float myReversingSpeed = 50f;

    [Tooltip("How long the roomba should reverse before rotating (in seconds)")]
    public float myBackwardMovingTimeTarget = 2f;

    [Header("Rotation settings")]

    [Tooltip("How long it takes for the roomba to rotate to the rotation (in seconds)")]
    public float myRotationTime = 3f;

    [Tooltip("What the minimum angle the roomba can rotate is (in degrees)")]
    public float myMinRotationAmount = 15f;

    [Tooltip("What the maximum angle the roomba can rotate is (in degrees)")]
    public float myMaxRotationAmount = 45f;

    float myCurrentMovementSpeed;
    float myTimeMovingBackward;

    private Vector3 myTargetRotationVector;

    float myTurnDirection;

    Quaternion myTargetRotationQuaternion;

    private bool myShouldMovebackwards = false;
    
    private bool myCanMove = true;
    private bool canChangeRotation = true;

    [HideInInspector]
    public bool myIsLookingForANewPath = false;

    private bool myHasToggledMoveBackwards;
    private bool myHasChangedRotation = false;

    private void Start()
    {
        ResetCurrentSpeed();

        myTargetRotationVector = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if(myCanMove)
        {
            switch (myShouldMovebackwards)
            {
                case true:
                    myCurrentMovementSpeed = myReversingSpeed / 100;
                    Move(-myCurrentMovementSpeed);
                    break;
                case false:
                    Move(myCurrentMovementSpeed);
                    break;
            }
        }
       
        if(myIsLookingForANewPath)
        {
            LookForNewPath();
        }
    }

    void LookForNewPath()
    {
        if(!myHasToggledMoveBackwards)
        {
            ToggleMoveBackwards();
            myHasToggledMoveBackwards = true;
        }

        if (myTimeMovingBackward < myBackwardMovingTimeTarget)
        {
            myTimeMovingBackward += Time.deltaTime;
        }
        else
        {
            myCanMove = false;

            if(!myHasChangedRotation)
            {
                ChangeTargetRotation();
                myHasChangedRotation = true;
            }

            if (Quaternion.Angle(myTargetRotationQuaternion, transform.rotation) > 0.01f)
            {
                Rotate();
            }
            else
            {
                FoundNewPath();
            }
        }
    }

    void FoundNewPath()
    {
        ToggleMoveBackwards();
        ResetCurrentSpeed();
        myHasToggledMoveBackwards = false;
        myCanMove = true;
        myIsLookingForANewPath = false;
        myTimeMovingBackward = 0;
        myHasChangedRotation = false;
        canChangeRotation = true;
    }

    void ChangeTargetRotation()
    {

        if (canChangeRotation)
        {
            myTurnDirection = Random.Range(0.0f, 1.0f);
            
            if(myTurnDirection >= 0.5f)
            {
                myTargetRotationVector.y += (Random.Range(myMinRotationAmount, myMaxRotationAmount) + 0.001f);
                myTargetRotationQuaternion = Quaternion.Euler(myTargetRotationVector);
            }
            else if(myTurnDirection < 0.5f)
            {
                myTargetRotationVector.y -= (Random.Range(myMinRotationAmount, myMaxRotationAmount) + 0.001f);
                myTargetRotationQuaternion = Quaternion.Euler(myTargetRotationVector);

            }

            canChangeRotation = false;
        }
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, myTargetRotationQuaternion, myRotationTime * Time.deltaTime);

        if (Vector3.Distance(transform.eulerAngles, myTargetRotationVector) <= 0.1f)
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
