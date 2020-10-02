using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform myTarget;
    //[SerializeField] Vector3 myTargetOffset;

    [SerializeField] private float myDistanceToTargetX = 3;
    [SerializeField] private float myDistanceToTargetY = 1;

    [SerializeField] private AnimationCurve myLookAtSpeedCurve;
    private Vector3 myTargetLookAt;

    [SerializeField] private AnimationCurve myMoveSpeedCurve;
    private Vector3 myTargetPos;

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

        float angleOfTarget = Vector3.Angle(myTarget.up, Vector3.up);

        targetPosition.x /= angleOfTarget;
        targetPosition.z /= angleOfTarget;

        //transform.position = targetPosition + myTargetOffset;
        transform.position = targetPosition;
        //transform.position = targetPosition - (myTarget.forward + new Vector3(0f,0f,2f)) * 3;

    }

    void SetRotation()
    {
        //transform.LookAt(myTarget.transform.position + myTarget.forward);
        //Vector3 newLookAt = new Vector3();
        //transform.LookAt(myTarget.transform.position + (myTarget.transform.up + myTarget.forward) *0.5f);
        transform.LookAt(myTarget.transform.position);
    }
}
