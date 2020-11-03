using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    StageManager myStageManager;
    [SerializeField] Transform myPlayer;
    [SerializeField] Vector3 myOffset = new Vector3(0, .5f, 0);
    [SerializeField] float myRotationSpeed = 50;
    Transform myCurrentTarget;

    private void Start()
    {
        myStageManager = StageManager.ourInstance;
        myCurrentTarget = myStageManager.myFirstBlock.transform;
        myStageManager.OnPickup += OnPickup;
        transform.parent = null;
    }

    void Update()
    {
        transform.position = myPlayer.position + myOffset;
        transform.LookAt(transform);
        Quaternion targetRotation = Quaternion.LookRotation(myCurrentTarget.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * myRotationSpeed);
    }

    void OnPickup(Transform newTarget)
    {
        myCurrentTarget = newTarget;
    }
}
