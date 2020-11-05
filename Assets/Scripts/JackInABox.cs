using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInABox : MonoBehaviour
{
    [SerializeField] public AnimationCurve myAnimationCurve;
    [SerializeField] public GameObject myHead;
    [SerializeField] public GameObject myBoxLidAnchorPoint;
    [SerializeField] public GameObject myCrank;
    [SerializeField] public bool repeatAnimation = false;

    protected float myTimeCounter = 0f;
    protected bool myFirstRun = true;
    protected bool myIsTriggered = false;

    protected float myLerpTime = 2.0f;
    protected Vector3 myStartScale = new Vector3(1f, 0.1f, 1f);
    protected Vector3 myEndScale = new Vector3(1f, 3f, 1f);
    protected Vector3 myHeadStartPosition;
    protected Vector3 mySpringStartPos;
    protected Vector3 myLidStartPos;
    protected Quaternion myLidStartRot;
    protected Quaternion myLidAnchorPointStartRot;
    protected Transform myOldSpringTransform;
    protected Transform myLidTransform;
    
    [SerializeField] protected Transform myRootTransform;
    protected Vector3 myRootStartPos;
    protected Quaternion myRootStartRot;

    private void Start()
    {
        
        myOldSpringTransform = transform.GetChild(0).transform; // child has the actual mesh renderer
        myLidTransform = myBoxLidAnchorPoint.transform.GetChild(0).transform;

        myHeadStartPosition = myHead.transform.position;
        mySpringStartPos = myOldSpringTransform.position;
        myLidStartPos = myLidTransform.position;
        myLidStartRot = myLidTransform.rotation;
        myLidAnchorPointStartRot = myBoxLidAnchorPoint.transform.rotation;

        myRootStartPos = myRootTransform.position;
        myRootStartRot = myRootTransform.rotation;
                
    }

    private void Update()
    {
        if (myIsTriggered)
        {
            // Hack for opening lid manually... by doing so we are not relying solely on physics and hinge joint on their own... which I would have preferred.
            if (myFirstRun)
            {
                myFirstRun = false;
                //myLidTransform.transform.Rotate(new Vector3(0f, 0f, -90f));
                myBoxLidAnchorPoint.transform.Rotate(new Vector3(0f, 0f, -90f));
                GameManager.ourInstance.myAudioManager.PlaySFXClip("jack_activate");
                StageManager.ourInstance.myOnResetBlocksAfterCheckpoint.AddListener(ResetStartPosition);
            }

            if (myTimeCounter < myLerpTime)
            {
                myTimeCounter += Time.deltaTime;
                if (myTimeCounter > myLerpTime)
                {
                    // Freeze counter after exceeding myLerpTime    
                    myTimeCounter = myLerpTime;
                }
            }


            // Scale the spring
            float lerpRatio = myTimeCounter / myLerpTime; // x value of animation curve
            float curveFactor = myAnimationCurve.Evaluate(lerpRatio); // y value of animation curve
            var scale = Vector3.Lerp(myStartScale, myEndScale, curveFactor);
            transform.localScale = scale;

            // Rotate my crank
            if (myTimeCounter < myLerpTime)
            {
                myCrank.transform.Rotate(new Vector3(0,0,1), 5/ myTimeCounter);
            }

            // Adjust head position
            var newSpringCenterOffset = myOldSpringTransform.position - mySpringStartPos;
            myHead.transform.position = myHeadStartPosition + newSpringCenterOffset * 2;

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    ResetStartPosition();
            //}
        }
    }

    virtual public void Trigger()
    {
        myIsTriggered = true;
    }

    


    virtual public void ResetStartPosition()
    {
        StageManager.ourInstance.myOnResetBlocksAfterCheckpoint.RemoveListener(ResetStartPosition);
        myIsTriggered = false;
        myFirstRun = true;
        myTimeCounter = 0;
        transform.localScale = myStartScale;

        myHead.transform.position = myHeadStartPosition;
        myBoxLidAnchorPoint.transform.rotation = myLidAnchorPointStartRot;
        myLidTransform.position = myLidStartPos;
        myLidTransform.rotation = myLidStartRot;
        
        myOldSpringTransform.position = mySpringStartPos;

    }
}
