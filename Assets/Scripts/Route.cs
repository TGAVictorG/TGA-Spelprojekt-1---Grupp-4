using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Based on:
 * https://www.youtube.com/watch?v=11ofnLOE8pw
 */

public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] myControlPoints;
    private Vector3 myGizmoPosition;

    private void OnDrawGizmos()
    {
        // A cubic Bezier curve
        // https://en.wikipedia.org/wiki/B%C3%A9zier_curve#Cubic_B%C3%A9zier_curves
        var points = myControlPoints;
        for (float t = 0; t <= 1; t += 0.01f)
        {
            myGizmoPosition = Mathf.Pow(1 - t, 3) * points[0].position +
                              3 * Mathf.Pow(1 - t, 2) * t * points[1].position +
                              3 * (1 - t) * Mathf.Pow(t, 2) * points[2].position +
                              Mathf.Pow(t, 3) * points[3].position;

            Gizmos.DrawSphere(myGizmoPosition, 0.05f);
        }

        var from0 = new Vector3(points[0].position.x, points[0].position.y, points[0].position.z);
        var to1 = new Vector3(points[1].position.x, points[1].position.y, points[1].position.z);
        Gizmos.DrawLine(from0, to1);

        var from2 = new Vector3(points[2].position.x, points[2].position.y, points[2].position.z);
        var to3 = new Vector3(points[3].position.x, points[3].position.y, points[3].position.z);
        Gizmos.DrawLine(from2, to3);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
