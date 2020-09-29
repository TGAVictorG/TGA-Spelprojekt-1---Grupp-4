using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform myTarget;
    [SerializeField] Vector3 myTargetOffset;

    [SerializeField] private float myDistanceToTarget = 3;

    // Update is called once per frame
    void Update()
    {
        SetPosition();
        SetRotation();
    }

    void SetPosition()
    {
        Vector3 targetPosition = myTarget.position - new Vector3(myTarget.forward.x, 0.0f, myTarget.forward.z).normalized * myDistanceToTarget;
        //targetPosition.y -= myTarget.forward.y;

        ////targetPosition.Normalize();
        //targetPosition *= myDistanceToTarget;

        ////targetPosition.y = 0;


        targetPosition.y = myTarget.position.y;



        //transform.position = targetPosition + myTargetOffset;
        transform.position = targetPosition;
        //transform.position = targetPosition - (myTarget.forward + new Vector3(0f,0f,2f)) * 3;

    }

    void SetRotation()
    {
        //transform.LookAt(myTarget.transform.position + myTarget.forward);
        //Vector3 newLookAt = new Vector3();
        transform.LookAt(myTarget.transform.position);
    }
}
