using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GuideArrow : MonoBehaviour
{
    StageManager myStageManager;

    [SerializeField] Transform myPlayer;
    [SerializeField] Vector3 myArrowOffset = new Vector3(0, .5f, 0);
    [SerializeField] Vector3 myLineOffset = new Vector3(0.004f, 0f, 0.4f);
    [SerializeField] float myRotationSpeed = 50;

    [SerializeField] LineRenderer myGuideLine;

    Transform myCurrentTarget;
    
private void Start()
    {
        myStageManager = StageManager.ourInstance;
        myCurrentTarget = myStageManager.myFirstBlock.transform;
        myStageManager.OnPickup += OnPickup;
        myStageManager.myOnPlayerRestartCheckpoint.AddListener(OnRestartFromCheckpoint);
        transform.parent = null;
    }

    void Update()
    {
        transform.position = myPlayer.position + myArrowOffset;
        transform.LookAt(transform);
        Quaternion targetRotation = Quaternion.LookRotation(myCurrentTarget.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * myRotationSpeed);

        myGuideLine.SetPosition(0, myPlayer.position);
        myGuideLine.SetPosition(1, myCurrentTarget.position);
    }

    void OnPickup(Transform newTarget)
    {
        myCurrentTarget = newTarget;
    }
    void OnRestartFromCheckpoint()
    {
        Debug.Assert(myStageManager.myCurrentCheckpoint != null, "Current checkpoint is null in StageManager!");
        myCurrentTarget = myStageManager.myCurrentCheckpoint.GetComponent<PickupScript>().myNextTarget.transform;
    }
}
