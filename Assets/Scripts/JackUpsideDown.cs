using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackUpsideDown : JackInABox
{    
    private float myTimeCounter = 0f;
    private bool myFirstRun = true;
    private bool myAnimationIsPaused = false;
    private bool myIsTriggered = false;
        
    [SerializeField] bool myForceJump = false;
    [SerializeField] Rigidbody myForceRigidbody;
    [SerializeField] Vector3 myForce;
    [SerializeField] bool isDebugging = false;
    [SerializeField] private bool useImpulseForceMode = true;

    private float myLerpTime = 3.0f;
    private Vector3 myStartScale = new Vector3(1f, 0f, 1f);
    private Vector3 myEndScale = new Vector3(1f, 3f, 1f);
    private Vector3 myHeadStartPosition;
    private Vector3 mySpringMeshStartPos;
    private Vector3 myRelHeadStartPos;
    private Vector3 myLidStartPosition;
    private Vector3 myAnchorPosition;
    [SerializeField] private Transform mySpringMeshTransform;
    private Transform myLidTransform;

    [SerializeField ] private Transform mySpringTransform;
    [SerializeField] private Transform myAnchorTransform; // Actually the same as mySpringTransform

    void Start()
    {
        myAnchorPosition = myAnchorTransform.position;
        myHeadStartPosition = myHead.transform.position;
        //mySpringMeshTransform = mySpringMeshTransform.GetChild(0).transform; // child has the actual mesh renderer
        mySpringMeshStartPos = mySpringMeshTransform.position;
        myRelHeadStartPos = myHeadStartPosition - myAnchorPosition;
        myLidTransform = myBoxLidAnchorPoint.transform.GetChild(0).transform;
        myLidStartPosition = myLidTransform.position;


    }

    void Update() {
        DebugJump();

        if (myIsTriggered)
        {
            // Hack for opening lid manually... by doing so we are not relying solely on physics and hinge joint on their own... which I would have preferred.
            if (myFirstRun)
            {
                myFirstRun = false;
                myBoxLidAnchorPoint.transform.Rotate(new Vector3(0f, 0f, -90f));
                GameManager.ourInstance.myAudioManager.PlaySFXClip("jack_jump");
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
            
            // Scale the spring
            float lerpRatio = myTimeCounter / myLerpTime; // x value of animation curve
            float curveFactor = myAnimationCurve.Evaluate(lerpRatio); // y value of animation curve
            var scale = Vector3.Lerp(myStartScale, myEndScale, curveFactor);
            mySpringTransform.localScale = scale;

            // Rotate my crank
            if (myTimeCounter < myLerpTime)
            {
                myCrank.transform.Rotate(new Vector3(0, 0, 1), 20 / myTimeCounter);
            }

            // Adjust head position
            myAnchorPosition = myAnchorTransform.position;
            var newChildCenterOffset = mySpringMeshTransform.position - myAnchorPosition;
            myHead.transform.position = myAnchorPosition + myRelHeadStartPos + newChildCenterOffset * 2;
        }
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

