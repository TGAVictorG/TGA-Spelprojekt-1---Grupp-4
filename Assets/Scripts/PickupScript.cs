using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{

    [SerializeField] private float myRotatingSpeed = 50;
    [SerializeField] private PickupScript myNextTarget = null;

    GameObject myTargetIndication = null;

    //Visar en cylinder till nästa pickup. Endast i Runtime.
    GameObject myDebugLine = null;
    [SerializeField] private bool myDebugLineIsVisable;

    void Start()
    {
        //debug-kod för LD
        if (myNextTarget != null)
        {
            myDebugLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            myDebugLine.GetComponent<Collider>().enabled = false;
            myDebugLine.transform.localScale = new Vector3(0.2f, (myNextTarget.transform.position - transform.position).magnitude / 2, 0.2f);
            myDebugLine.transform.position = transform.position + (myNextTarget.transform.position - transform.position) / 2;
            myDebugLine.transform.up = (myNextTarget.transform.position - transform.position).normalized;
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, myRotatingSpeed * Time.deltaTime);


        //debug-kod för LD
        if (myDebugLine != null)
        {
            myDebugLine.GetComponent<Renderer>().enabled = myDebugLineIsVisable;
        }
    }

    private void OnTriggerEnter(Collider aPlayer)
    {

        if (myNextTarget != null)
        {
            myTargetIndication = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            myTargetIndication.GetComponent<Collider>().enabled = false;
            myTargetIndication.transform.parent = myNextTarget.transform;
            myTargetIndication.transform.position = myNextTarget.transform.position;
            Destroy(gameObject);
            Destroy(myDebugLine);
        }
    }
}
