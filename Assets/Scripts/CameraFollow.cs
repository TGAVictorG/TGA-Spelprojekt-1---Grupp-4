using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform myTarget;
    [SerializeField] Vector3 myTargetOffset;
    [SerializeField] bool myUseWorldOffset;

    // Update is called once per frame
    void Update()
    {
        SetPosition();
        SetRotation();
    }

    void SetPosition()
    {
        Vector3 targetPosition = myTarget.position;

        if(!myUseWorldOffset)
        {
            transform.position = targetPosition + myTargetOffset;
        }
        else
        {
            transform.position = myTarget.TransformPoint(myTargetOffset);
        }
    }

    void SetRotation()
    {
        transform.LookAt(myTarget);
    }
}
