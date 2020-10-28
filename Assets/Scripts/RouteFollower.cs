using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/**
 * Based on:
 * https://www.youtube.com/watch?v=11ofnLOE8pw
 */

public class RouteFollower : MonoBehaviour
{
    public Transform[] myRoutes { get; set; }
    private int myCurrentRoute;
    private float myParam;
    [SerializeField] public float myManualParamOffsetHack = 0f;
    public float myStartingParam { get; set; } = 0f;    
    public float mySpeedFactor { get; set; } = 1f;

    private bool myCoroutineAllowed;

    void Awake()
    {
        myCurrentRoute = 0;        
        myCoroutineAllowed = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        myParam = myStartingParam + myManualParamOffsetHack;
    }

    // Update is called once per frame
    void Update()
    {
        if (myCoroutineAllowed)
        {
            StartCoroutine(TravelRoute(myCurrentRoute));
        }
    }       

    private IEnumerator TravelRoute(int aRouteNumber)
    {
        while (myRoutes == null)
        {
            yield return null;
        }
        myCoroutineAllowed = false;
        Vector3 p0 = myRoutes[myCurrentRoute].GetChild(0).position;
        Vector3 p1 = myRoutes[myCurrentRoute].GetChild(1).position;
        Vector3 p2 = myRoutes[myCurrentRoute].GetChild(2).position;
        Vector3 p3 = myRoutes[myCurrentRoute].GetChild(3).position;

        var myNextRoute = (myCurrentRoute + 1) % myRoutes.Length;
        Vector3 q0 = myRoutes[myNextRoute].GetChild(0).position;
        Vector3 q1 = myRoutes[myNextRoute].GetChild(1).position;
        Vector3 q2 = myRoutes[myNextRoute].GetChild(2).position;
        Vector3 q3 = myRoutes[myNextRoute].GetChild(3).position;        

        while (myParam < 1)
        {
            // https://gamedev.stackexchange.com/questions/27056/how-to-achieve-uniform-speed-of-movement-on-a-bezier-curve
            float stepSize = Time.deltaTime * mySpeedFactor;
            float t = myParam;
            // Cubic Bezier derivative
            Vector3 dB1dt = 3 * Mathf.Pow(1 - t, 2) * (p1 - p0) + 6 * (1 - t) * t * (p2 - p1) + 3 * Mathf.Pow(t, 2) * (p3 - p2);
            //t += stepSize / dBdt.magnitude;
            float dt = stepSize / dB1dt.magnitude;

            Vector3 nextPos;

            if (t + dt <= 1) // On current route
            {
                t += dt;
                nextPos = Mathf.Pow(1 - t, 3) * p0 +
                          3 * Mathf.Pow(1 - t, 2) * t * p1 +
                          3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                          Mathf.Pow(t, 3) * p3;
            }
            else // Overshoot into next route
            {                
                float dt1 = 1 - t;                
                float L1 = dt1 * dB1dt.magnitude; // Length traveled on currentRoute
                float L2 = stepSize - L1; // The remaining lenght to travel on the nextRoute
                
                // Take the derivative in t2 = 0, simplifies the expression:
                //      Vector3 dB2dt = 3 * Mathf.Pow(1 - t2, 2) * (q1 - q0) + 6 * (1 - t2) * t2 * (q2 - q1) + 3 * Mathf.Pow(t2, 2) * (q3 - q2); 
                // into:
                Vector3 dB2dt = 3 * (q1 - q0);
                float dt2 = L2 / dB2dt.magnitude;
                
                // Next position is B2(t) where t = dt2
                nextPos = Mathf.Pow(1 - dt2, 3) * q0 +
                          3 * Mathf.Pow(1 - dt2, 2) * dt2 * q1 +
                          3 * (1 - dt2) * Mathf.Pow(dt2, 2) * q2 +
                          Mathf.Pow(dt2, 3) * q3; ;
                t = 1 + dt2;
            }

            // Turn train in the movement direction
            transform.LookAt(nextPos);
            transform.position = nextPos;
            myParam = t;

            yield return null;
        }

        myParam -= 1.0f; // Keep the remainder for the next route

        myCurrentRoute = myNextRoute;
                
        myCoroutineAllowed = true;
    } 

}
