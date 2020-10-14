using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{

    [SerializeField] private float myRotatingSpeed = 50;
    [SerializeField] private PickupScript myNextTarget = null;
    [SerializeField] private float myFuelToAdd = 2.0f;    
    [SerializeField] private float mySpeedBoost = 2.0f;

    private GameObject myHalo;

    //Visar en cylinder till nästa pickup. Endast i Runtime.
    GameObject myDebugLine = null;
    [SerializeField] private bool myDebugLineIsVisable;
    [Tooltip("Can collect this pickup in any order.")]
    [SerializeField] private bool myDebugIsCollectible = true;

    void Start()
    {
        // Debug: collect in any order
        if (myDebugIsCollectible) {
            GetComponent<Collider>().enabled = true;
        }

        myHalo = transform.GetChild(0).gameObject;
        
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
        aPlayer.gameObject.GetComponent<SpeedBoost>().ActivateSpeedBoost(mySpeedBoost);

        StageManager.ourInstance.OnPickedUpBlock();
        Destroy(gameObject);
    }

    private void ActivateMeAsTarget()
    {
        // Make me collectible
        GetComponent<Collider>().enabled = true;

        // Make me glow        
        var material = gameObject.GetComponent<Renderer>().material;

        // Activate emission on material        
        material.EnableKeyword("_EMISSION");    

        // Set material's Rendering mode to transparent
        //https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        Behaviour halo = (Behaviour) myHalo.GetComponent("Halo");
        halo.enabled = true;
    }
}

