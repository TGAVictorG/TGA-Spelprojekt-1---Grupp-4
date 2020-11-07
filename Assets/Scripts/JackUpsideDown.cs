using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackUpsideDown : JackInABox
{    
    [SerializeField] bool myForceJump = false;
    [SerializeField] Rigidbody myForceRigidbody;
    [SerializeField] Vector3 myForce;
    [SerializeField] bool isDebugging = false;
    [SerializeField] private bool useImpulseForceMode = true;

    private Vector3 myRelHeadStartPos;
    private Vector3 myAnchorPosition;
    [SerializeField] private Transform mySpringMeshTransform;

    [SerializeField] private Transform mySpringAnchorTransform;

    void Start()
    {
        myAnchorPosition = mySpringAnchorTransform.position;
        myHeadStartPosition = myHead.transform.position;
        myRelHeadStartPos = myHeadStartPosition - myAnchorPosition;
        myLidTransform = myBoxLidAnchorPoint.transform.GetChild(0).transform;
        myLidStartPos = myLidTransform.position;
        myLidStartRot = myLidTransform.rotation;

        mySpringStartPos = mySpringMeshTransform.position;
        myLidAnchorPointStartRot = myBoxLidAnchorPoint.transform.rotation;
        

        myRootStartPos = myRootTransform.position;
        myRootStartRot = myRootTransform.rotation;
    }

    void Update() {
        //DebugJump();

        if (myIsTriggered)
        {
            // Hack for opening lid manually... by doing so we are not relying solely on physics and hinge joint on their own... which I would have preferred.
            if (myFirstRun)
            {
                myFirstRun = false;
                myBoxLidAnchorPoint.transform.Rotate(new Vector3(0f, 0f, -90f));
                GameManager.ourInstance.myAudioManager.PlaySFXClip("jack_jump");

                HingeJoint hinge = GetComponentInChildren<HingeJoint>();
                JointLimits limits = hinge.limits;
                limits.min = -75;
                limits.max = -130;
                hinge.limits = limits;
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
            mySpringAnchorTransform.localScale = scale;

            // Rotate my crank
            if (myTimeCounter < myLerpTime)
            {
                myCrank.transform.Rotate(new Vector3(0, 0, 1), 20 / myTimeCounter);
            }

            // Adjust head position
            myAnchorPosition = mySpringAnchorTransform.position;
            var newSpringCenterOffset = mySpringMeshTransform.position - myAnchorPosition;
            myHead.transform.position = myAnchorPosition + myRelHeadStartPos + newSpringCenterOffset * 2;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetStartPosition();
            }
        }
    }

    override protected void ResetStartPosition() 
    {
        StageManager.ourInstance.myOnResetAtRespawn.RemoveListener(ResetStartPosition);
        myIsTriggered = false;
        myFirstRun = true;
        myTimeCounter = 0;

        mySpringAnchorTransform.localScale = myStartScale;

        myRootTransform.position = myRootStartPos;
        myRootTransform.rotation = myRootStartRot;

        myHead.transform.position = myHeadStartPosition;
        myBoxLidAnchorPoint.transform.rotation = myLidAnchorPointStartRot;
        myLidTransform.position = myLidStartPos;
        myLidTransform.rotation = myLidStartRot;

        mySpringMeshTransform.position = mySpringStartPos;

        HingeJoint hinge = GetComponentInChildren<HingeJoint>();
        JointLimits limits = hinge.limits;
        limits.min = 0;
        limits.max = 0;
        hinge.limits = limits;

    }

    private void DebugJump() {
        if (isDebugging && Input.GetKeyDown(KeyCode.Space)) {
            if (myIsTriggered) {
                myIsTriggered = false;

                Trigger();
            }
        }
    }

    override public void Trigger()
    {        
        if (myForceJump && !myIsTriggered)
        {
            if (useImpulseForceMode) {
                myForceRigidbody.AddForce(myForce * 10, ForceMode.Impulse);
            }
            else {
                myForceRigidbody.AddForce(myForce * 10);
            }

            
        }
        myIsTriggered = true;
    }

    void OnDrawGizmosSelected()
    {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position+myForce*0.6f);
    }
}

