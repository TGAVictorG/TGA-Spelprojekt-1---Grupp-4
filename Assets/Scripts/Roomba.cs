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

    public float myMinRotationAmount = 15f;
    public float myMaxRotationAmount = 45f;

    float myCurrentMovementSpeed;
    float myTimeMovingBackward;

    Rigidbody myRigidbody;

    public Vector3 targetRotationVector;

    int timesRotationHasChanged = 1;
    float myTurnDirection;

    Quaternion myTargetRotationQuaternion;

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

        //if(Input.GetMouseButtonDown(0))
        //{
        //    targetRotationVector.y += myRotationDegrees;
        //}
    }

    void LookForNewPath()
    {
        if(!hasToggledMoveBackwards)
        {
            ToggleMoveBackwards();
            hasToggledMoveBackwards = true;
           // Debug.Log("Toggling move backwards");
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

            
            //if(targetRotationVector.y >= 359)
            //{
            //    Rotate();
            //    if(transform.rotation.y >= 359 || transform.rotation.eulerAngles.y <= 0)
            //    {
            //        transform.eulerAngles = Vector3.zero;
            //        targetRotationVector = Vector3.zero;
            //
            //        FoundNewPath();
            //
            //    }
            //}

            if (Quaternion.Angle(myTargetRotationQuaternion, transform.rotation) > 0.01f)
            {
                Rotate();
            }
            else
            {
                //targetRotationVector.y = transform.eulerAngles.y;
                FoundNewPath();
            }
            //if (Mathf.Ceil(transform.rotation.eulerAngles.y) < targetRotationVector.y)
            //{
            //    
            //    Debug.Log(Mathf.Ceil(transform.rotation.eulerAngles.y));
            //
            //}
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
        //Debug.Log("Changing target rotation");

        if (canChangeRotation)
        {

            myTurnDirection = Random.Range(0.0f, 1.0f);

            
            if(myTurnDirection >= 0.5f)
            {
                targetRotationVector.y += (Random.Range(myMinRotationAmount, myMaxRotationAmount) + 0.001f);
                myTargetRotationQuaternion = Quaternion.Euler(targetRotationVector);
                //Debug.Log(Quaternion.Angle(myTargetRotationQuaternion, transform.rotation));
            }
            else if(myTurnDirection < 0.5f)
            {
                targetRotationVector.y -= (Random.Range(myMinRotationAmount, myMaxRotationAmount) + 0.001f);
                myTargetRotationQuaternion = Quaternion.Euler(targetRotationVector);
               // Debug.Log(Quaternion.Angle(myTargetRotationQuaternion, transform.rotation));

                //Debug.Log(Vector3.Distance(new Vector3(0, Mathf.Abs(transform.eulerAngles.y), 0), new Vector3(0, Mathf.Abs(targetRotationVector.y), 0)));
            }




            //transform.localEulerAngles = Vector3.zero;
            //targetRotationVector = Vector3.zero;

            canChangeRotation = false;
        }
    }

    void Rotate()
    {
        
        transform.rotation = Quaternion.Lerp(transform.rotation, myTargetRotationQuaternion, myRotationTime * Time.deltaTime);


        if (Vector3.Distance(transform.eulerAngles, targetRotationVector) <= 0.1f)
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
