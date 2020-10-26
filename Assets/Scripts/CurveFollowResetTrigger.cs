using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveFollowResetTrigger : MonoBehaviour
{
  
    bool shouldMoveFishBack = false;

    CurveController curveControllerToReset;
    Vector3 targetRotation;
    float LerpDuration;
    float timeElapsed = 0;

    bool hasRotated = false;

    private void Update()
    {
        
        if(shouldMoveFishBack)
        {

            Vector3 targetPosition = curveControllerToReset.myCurveRoot.transform.GetChild(0).position;
            LerpDuration = curveControllerToReset.mySwimBackTime;
            //curveControllerToReset.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

            // curveControllerToReset.enabled = false;



            if (timeElapsed < LerpDuration)
            {
                curveControllerToReset.transform.position = Vector3.Lerp(transform.position, targetPosition, timeElapsed/LerpDuration);
                //curveControllerToReset.transform.LookAt(targetPosition);
                //curveControllerToReset.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                
                if(hasRotated == false)
                {
                    curveControllerToReset.transform.Rotate(new Vector3(transform.rotation.x, transform.eulerAngles.y + 180, transform.rotation.z));
                    hasRotated = true;
                }


                timeElapsed += Time.deltaTime;
            }
            else
            {
                shouldMoveFishBack = false;
                hasRotated = false;
                curveControllerToReset.enabled = true;
                curveControllerToReset.FollowCurve();
                curveControllerToReset.transform.Rotate(new Vector3(transform.rotation.x, transform.eulerAngles.y + 180, transform.rotation.z));
                curveControllerToReset.PlayUpSound();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<CurveController>()!= null)
        {

            curveControllerToReset = other.gameObject.GetComponent<CurveController>();
            curveControllerToReset.PlayDownSound();
            targetRotation = new Vector3(curveControllerToReset.transform.rotation.x, 180, curveControllerToReset.transform.rotation.z);
            shouldMoveFishBack = true;
            timeElapsed = 0;



           // other.gameObject.GetComponent<CurveController>().FollowCurve();
        }
    }
}
