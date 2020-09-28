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

    [SerializeField] private float myCoolDown = 1f;
    private float myCoolDownCounter = 0f;
    
    private float myLerpTime = 2.0f;
    private Vector3 myStartScale = new Vector3(1f,0f,1f);
    private Vector3 myEndScale = new Vector3(1f, 3f, 1f);
    private Vector3 myHeadStartPosition;
    private Vector3 myChildStartPosition;
    private Transform myChildTransform;

    // Start is called before the first frame update
    void Start()
    {        
        myHeadStartPosition = myHead.transform.position;
        myChildTransform = transform.GetChild(0).transform; // child has the actual mesh renderer
        myChildStartPosition = myChildTransform.position;
    }

    // Update is called once per frame
    void Update()
    {       
        myTimeCounter += Time.deltaTime;

        if (myTimeCounter > myLerpTime) {
            myTimeCounter = myLerpTime;

            // For debugging and tweaking purposes:
            if (repeatAnimation) {
                myCoolDownCounter += Time.deltaTime;
                if (myCoolDownCounter > myCoolDown) {
                    myCoolDownCounter = 0f;
                    myTimeCounter = 0f;
                }
            }
        }

        // Hack for opening lid manually... by doing so we are not relying solely on physics and hinge joint on their own... which I would have preferred.
        if (myFirstRun) {
            myFirstRun = false;
            myBoxLidAnchorPoint.transform.Rotate(new Vector3(90f, 0f, 0f));
        }

        float lerpRatio = myTimeCounter / myLerpTime; // x value of animation curve
        float curveFactor = myAnimationCurve.Evaluate(lerpRatio); // y value of animation curve
        var scale = Vector3.Lerp(myStartScale, myEndScale, curveFactor);
        transform.localScale = scale;

        // Adjust head position
        var newChildCenterOffset = myChildTransform.position - myChildStartPosition;
        myHead.transform.position = myHeadStartPosition + newChildCenterOffset * 2;
    }    
}
