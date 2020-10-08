using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    [SerializeField] private float myZoomValue = 0.5f;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider anOther)
    {
        if (anOther.tag == "Player")
        {

            Camera.main.GetComponent<CameraFollow>().myDistanceToTargetUp *= myZoomValue;
            Camera.main.GetComponent<CameraFollow>().myDistanceToTargetBack *= myZoomValue;
        }
    }

    private void OnTriggerExit(Collider anOther)
    {
        if (anOther.tag == "Player")
        {

            Camera.main.GetComponent<CameraFollow>().myDistanceToTargetUp /= myZoomValue;
            Camera.main.GetComponent<CameraFollow>().myDistanceToTargetBack /= myZoomValue;
        }
    }
}
