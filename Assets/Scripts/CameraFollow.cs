using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform myTarget;
    //[SerializeField] Vector3 myTargetOffset;

    [SerializeField] public float myDistanceToTargetUp = 3;
    [SerializeField] public float myDistanceToTargetBack = 1;

    //[SerializeField] public float myMaxDistanceToTarget = 5;

    [SerializeField] private AnimationCurve myLookAtSpeedCurve;
    private Vector3 myTargetLookAt;

    [SerializeField] private AnimationCurve myMoveSpeedCurve;
    private Vector3 myTargetPos;

    private Vector3 myLastTargetPosISawTarget;


    private void Start()
    {
        gameObject.transform.position = myTarget.position;
        myLastTargetPosISawTarget = myTarget.position;
    }


    // Update is called once per frame
    void Update()
    {
        SetPosition();
        SetRotation();
    }

    void SetPosition()
    {
        float maxDistanceToTarget = new Vector2(myDistanceToTargetBack, myDistanceToTargetUp).magnitude;
        Vector3 targetPosition = myTarget.position - new Vector3(myTarget.forward.x, 0.0f, myTarget.forward.z).normalized * myDistanceToTargetBack;
        targetPosition.y = myTarget.position.y + myDistanceToTargetUp;

        //targetPosition = myTarget.position;

        float angleOfTarget = Vector3.Angle(new Vector3(myTarget.position.x, myTarget.position.y, 0f).normalized, Vector3.up);
        //print(myMoveSpeedCurve.Evaluate(angleOfTarget));
        //Vector3 goHereThisFrame = (targetPosition - transform.position) * 0.01f + transform.position;


        RaycastHit[] hits;
        Vector3 closestHit = Vector3.zero;
        Ray noClipRay = new Ray(myTarget.position, (targetPosition - myTarget.position));
        hits = Physics.RaycastAll(noClipRay, maxDistanceToTarget);

        float shortestHitDistance = -1;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //GameObject goHitall = hit.transform.gameObject;
            if (shortestHitDistance < 0 || hit.distance < shortestHitDistance)
            {
                shortestHitDistance = hit.distance;
                closestHit = hit.point;
            }
        }

        if (shortestHitDistance > 0)
        {
            targetPosition = closestHit;
        }
        //if(shortestHitDistance > myMaxDistanceToTarget)
        //{
        //    targetPosition += targetPosition.normalized * myMaxDistanceToTarget;
        //}

        Vector3 goHereThisFrame = (targetPosition - transform.position) * myMoveSpeedCurve.Evaluate(angleOfTarget) * Time.deltaTime + transform.position;
        //print(myTarget.localRotation.z/10);

        


        Debug.DrawRay(noClipRay.origin, targetPosition - noClipRay.origin);


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

        Vector3 lookHereThisFrame = (myTarget.position - transform.position) * myLookAtSpeedCurve.Evaluate(angleOfTarget) * Time.deltaTime + transform.position;

        transform.LookAt(myTarget.position);
    }
}
