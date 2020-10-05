using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMe : MonoBehaviour
{
    [SerializeField] private Transform myFollower;
    private Queue<Vector3> myPreviousPositions;
    private float myTimeCounter = 0f;
    [SerializeField] private float myFollowerTimeOffset = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        myPreviousPositions = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime > 0)
        {
            myTimeCounter += Time.deltaTime;
            myPreviousPositions.Enqueue(transform.position);
            if (myTimeCounter > myFollowerTimeOffset)
            {
                myFollower.position = myPreviousPositions.Dequeue();
                myFollower.LookAt(myPreviousPositions.Peek());
            }
            
        }
    }
}
