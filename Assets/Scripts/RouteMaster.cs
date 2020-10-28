using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMaster : MonoBehaviour
{
    [SerializeField] RouteFollower[] myRouteFollowers;
    [SerializeField] private Transform[] myRoutes;
    [SerializeField] public float mySpeedFactor = 1f;
    [SerializeField] private float myFollowerStartSpacing = 0.12f; // Changing this later in run-time after Route has begun will have no effect.

    private void Awake()
    {
        UpdateFollowerData();
    }

    void Start()
    {
        UpdateFollowerData();
    }

    private void UpdateFollowerData()
    {
        for (int i = 0; i < myRouteFollowers.Length; i++)
        {
            RouteFollower rf = myRouteFollowers[i]; 
            rf.mySpeedFactor = mySpeedFactor;
            rf.myStartingParam = (myRouteFollowers.Length - i) * myFollowerStartSpacing;
            rf.myRoutes = myRoutes;
        }
    }

    private void UpdateFollowerSpeed()
    {
        for (int i = 0; i < myRouteFollowers.Length; i++)
        {
            RouteFollower rf = myRouteFollowers[i];
            rf.mySpeedFactor = mySpeedFactor;
        }
    }    
}
