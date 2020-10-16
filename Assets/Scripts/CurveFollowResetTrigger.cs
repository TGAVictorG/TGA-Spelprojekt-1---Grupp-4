using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveFollowResetTrigger : MonoBehaviour
{
    public Transform myLastCurvePoint;

    private void Start()
    {
        if(myLastCurvePoint != null)
        {
            transform.position = myLastCurvePoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<CurveController>()!= null)
        {
            other.gameObject.GetComponent<CurveController>().FollowCurve();
        }
    }
}
