using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/**
 * Based on:
 * https://www.youtube.com/watch?v=11ofnLOE8pw
 */

public class RouteFollow : MonoBehaviour
{
    [SerializeField] private Transform[] myRoutes;
    private int myCurrentRoute;
    private float myParam;
    [SerializeField] [Range(0,0.5f)]float myStartingParam = 0f;    
    [SerializeField] private float mySpeedFactor = 1f;
    private bool myCoroutineAllowed;


    // Start is called before the first frame update
    void Start()
    {
        myCurrentRoute = 0;
        myParam = myStartingParam;
        myCoroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myCoroutineAllowed)
        {
            StartCoroutine(NewTravelRoute(myCurrentRoute));
        }
    }

    private IEnumerator NewTravelRoute(int aRouteNumber)
    {
        myCoroutineAllowed = false;
        Vector3 p0 = myRoutes[myCurrentRoute].GetChild(0).position;
        Vector3 p1 = myRoutes[myCurrentRoute].GetChild(1).position;
        Vector3 p2 = myRoutes[myCurrentRoute].GetChild(2).position;
        Vector3 p3 = myRoutes[myCurrentRoute].GetChild(3).position;        

        while (myParam < 1)
        {
            // https://gamedev.stackexchange.com/questions/27056/how-to-achieve-uniform-speed-of-movement-on-a-bezier-curve
            float stepSize = Time.deltaTime * mySpeedFactor;
            float t = myParam;
            // Cubic Bezier derivative
            Vector3 dBdt = 3 * Mathf.Pow(1 - t, 2) * (p1 - p0) + 6 * (1 - t) * t * (p2 - p1) + 3 * Mathf.Pow(t, 2) * (p3 - p2);
            t += stepSize / dBdt.magnitude;
                        
            if (t >= 0)
            {
                Vector3 nextPos = Mathf.Pow(1 - t, 3) * p0 +
                                  3 * Mathf.Pow(1 - t, 2) * t * p1 +
                                  3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                                  Mathf.Pow(t, 3) * p3;
                // Turn train in the movement direction
                transform.LookAt(nextPos);
                transform.position = nextPos;
            }
            myParam = t;

            yield return null;
        }

        myParam -= 1.0f; // Keep the remainder for the next route
        myCurrentRoute++;
        if (myCurrentRoute >= myRoutes.Length)
        {
            myCurrentRoute = 0;
        }

        myCoroutineAllowed = true;
    } 

}
