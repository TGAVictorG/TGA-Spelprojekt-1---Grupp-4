using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform myTarget;
    //[SerializeField] Vector3 myTargetOffset;

    [SerializeField] public float myDistanceToTargetX = 3;
    [SerializeField] public float myDistanceToTargetY = 1;

    [SerializeField] private AnimationCurve myLookAtSpeedCurve;
    private Vector3 myTargetLookAt;

    [SerializeField] private AnimationCurve myMoveSpeedCurve;
    private Vector3 myTargetPos;


    private void Start()
    {
        gameObject.transform.position = myTarget.position;
    }


    // Update is called once per frame
    void Update()
    {
        SetPosition();
        SetRotation();
    }

    void SetPosition()
    {
        Vector3 targetPosition = myTarget.position - new Vector3(myTarget.forward.x, 0.0f, myTarget.forward.z).normalized * myDistanceToTargetX;
        targetPosition.y = myTarget.position.y + myDistanceToTargetY;

        //targetPosition = myTarget.position;

        float angleOfTarget = Vector3.Angle(new Vector3(myTarget.position.x, myTarget.position.y, 0f).normalized, Vector3.up);
        print(myMoveSpeedCurve.Evaluate(angleOfTarget));
        //Vector3 goHereThisFrame = (targetPosition - transform.position) * 0.01f + transform.position;
        Vector3 goHereThisFrame = (targetPosition - transform.position) * myMoveSpeedCurve.Evaluate(angleOfTarget) / 100 + transform.position;
        //print(myTarget.localRotation.z/10);


        



        //transform.position = targetPosition + myTargetOffset;
        transform.position = goHereThisFrame;
        //transform.position = targetPosition - (myTarget.forward + new Vector3(0f,0f,2f)) * 3;

    }

    void SetRotation()
    {
        //transform.LookAt(myTarget.transform.position + myTarget.forward);
        //Vector3 newLookAt = new Vector3();
        //transform.LookAt(myTarget.transform.position + (myTarget.transform.up + myTarget.forward) *0.5f);

        float angleOfTarget = Vector3.Angle(new Vector3(myTarget.position.x, myTarget.position.y, 0f).normalized, Vector3.up);
        print(myMoveSpeedCurve.Evaluate(angleOfTarget));

        Vector3 lookHereThisFrame = (myTarget.position - transform.position) * myLookAtSpeedCurve.Evaluate(angleOfTarget) / 100 + transform.position;

        transform.LookAt(myTarget.position);
    }
}
