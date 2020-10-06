using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{

    [SerializeField] private float myRotatingSpeed = 50;
    [SerializeField] private PickupScript myNextTarget = null;
    [SerializeField] private float myFuelToAdd = 2.0f;
    [SerializeField] private Material myGlowMaterial;


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

    public void SetActive(bool aActive)
    {
        gameObject.SetActive(aActive);
        ActivateMeAsTarget();
    }

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
            myNextTarget.ActivateMeAsTarget();
            Destroy(myDebugLine);
        }

        aPlayer.gameObject.GetComponent<Fuel>().AddFuel(myFuelToAdd);

        StageManager.ourInstance.OnPickedUpBlock();
        Destroy(gameObject);
    }

    private void ActivateMeAsTarget()
    {
        // Make me collectible
        GetComponent<Collider>().enabled = true;

        // Make me glow
        GetComponent<MeshRenderer>().material = myGlowMaterial;
        Behaviour halo = (Behaviour)GetComponent("Halo");
        halo.enabled = true;
    }
}

