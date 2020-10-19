using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInABox : MonoBehaviour
{
    [SerializeField] public AnimationCurve myAnimationCurve;
    [SerializeField] public GameObject myHead;
    [SerializeField] public GameObject myBoxLidAnchorPoint;
    [SerializeField] public bool repeatAnimation = false;

    private float myTimeCounter = 0f;
    private bool myFirstRun = true;
    private bool myAnimationIsPaused = false;
    private bool myIsTriggered = false;

    [SerializeField] private float myCoolDownTime = 1f;
    [SerializeField] private float myAnimationPauseTime = 1f;
    private float myCoolDownCounter = 0f;
    private float myAnimationPauseCounter = 0f;

    private float myLerpTime = 2.0f;
    private Vector3 myStartScale = new Vector3(1f, 0f, 1f);
    private Vector3 myEndScale = new Vector3(1f, 3f, 1f);
    private Vector3 myHeadStartPosition;
    private Vector3 myChildStartPosition;
    private Vector3 myLidStartPosition;
    private Transform myChildTransform;
    private Transform myLidTransform;

    private void Start()
    {
        myHeadStartPosition = myHead.transform.position;
        myChildTransform = transform.GetChild(0).transform; // child has the actual mesh renderer
        myChildStartPosition = myChildTransform.position;
        myLidTransform = myBoxLidAnchorPoint.transform.GetChild(0).transform;
        myLidStartPosition = myLidTransform.position;
    }

    private void Update()
    {
        if (myIsTriggered)
        {
            // Hack for opening lid manually... by doing so we are not relying solely on physics and hinge joint on their own... which I would have preferred.
            if (myFirstRun)
            {
                myFirstRun = false;
                myBoxLidAnchorPoint.transform.Rotate(new Vector3(0f, 0f, -90f));
            }


            if (myTimeCounter < myLerpTime && !myAnimationIsPaused)
            {
                myTimeCounter += Time.deltaTime;
                if (myTimeCounter > myLerpTime)
                {
                    // Freeze counter after exceeding myLerpTime    
                    myTimeCounter = myLerpTime;
                }
            }
            else
            {

                // Repeat animaton for debugging and tweaking purposes:
                if (repeatAnimation)
                {
                    if (myAnimationIsPaused)
                    {
                        myAnimationPauseCounter += Time.deltaTime;
                        if (myAnimationPauseCounter > myAnimationPauseTime)
                        {
                            myAnimationPauseCounter = 0f;
                            myAnimationIsPaused = false;
                            myFirstRun = true;
                        }
                    }
                    else
                    {
                        myCoolDownCounter += Time.deltaTime;
                        if (myCoolDownCounter > myCoolDownTime)
                        {
                            myAnimationIsPaused = true;
                            myCoolDownCounter = 0f;
                            myTimeCounter = 0f;
                            myBoxLidAnchorPoint.transform.Rotate(new Vector3(0f, 0f, 0f));
                            myLidTransform.position = myLidStartPosition;
                        }
                    }
                }
            }

            // Scale the spring
            float lerpRatio = myTimeCounter / myLerpTime; // x value of animation curve
            float curveFactor = myAnimationCurve.Evaluate(lerpRatio); // y value of animation curve
            var scale = Vector3.Lerp(myStartScale, myEndScale, curveFactor);
            transform.localScale = scale;

            // Adjust head position
            var newChildCenterOffset = myChildTransform.position - myChildStartPosition;
            myHead.transform.position = myHeadStartPosition + newChildCenterOffset * 2;
        }
    }

    virtual public void Trigger()
    {
        myIsTriggered = true;
    }
}
