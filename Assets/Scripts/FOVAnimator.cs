using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVAnimator : MonoBehaviour
{

    [SerializeField]
    private float fovZoomInTime = 0.5f;

    [SerializeField]
    private float fovZoomOutTime = 0.5f;

    bool hasStartedFovZoom = false;
    float myToFov;

    Coroutine myZoomRoutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStartedFovZoom == true && Camera.main.fieldOfView == myToFov)
        {
            StopAllCoroutines();
            myZoomRoutine = StartCoroutine(LerpFieldOfView(60, fovZoomOutTime));
        }
    }

    public void ZoomFov(float aToFov)
    {
        myZoomRoutine = StartCoroutine(LerpFieldOfView(aToFov, fovZoomInTime));
        hasStartedFovZoom = true;
        myToFov = aToFov;
    }
    

    IEnumerator LerpFieldOfView(float aToFov, float aDuration)
    {
        float myCounter = 0;
        float myFromFov = Camera.main.fieldOfView;


        while (myCounter < aDuration)
        {
            myCounter += Time.deltaTime;

            float myFOVTime = myCounter / aDuration;

            Camera.main.fieldOfView = Mathf.Lerp(myFromFov, aToFov, myFOVTime);

            yield return null;
        }
    }
}
