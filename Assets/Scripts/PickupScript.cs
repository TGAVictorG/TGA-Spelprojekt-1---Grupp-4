﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{

    [SerializeField] private float myRotatingSpeed = 50;
    [SerializeField] private PickupScript myNextTarget = null;
    [SerializeField] private float myFuelToAdd = 2.0f;
    [SerializeField] private float mySpeedBoost = 2.0f;

    private GameObject myHalo;
    private Material myMaterial;

    //Visar en cylinder till nästa pickup. Endast i Runtime.
    GameObject myDebugLine = null;
    [SerializeField] private bool myDebugLineIsVisable;
    [Tooltip("Can collect this pickup in any order.")]
    [SerializeField] private bool myDebugIsCollectible = true;

    void Awake()
    {
        myHalo = transform.GetChild(0).gameObject;
        myMaterial = gameObject.GetComponent<Renderer>().material;
        SetMaterialTransparent(myMaterial);

    }

    void Start()
    {
        // Debug: collect in any order
        if (myDebugIsCollectible)
        {
            GetComponent<Collider>().enabled = true;
        }

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
        if (aPlayer.CompareTag("Player"))
        {
            if (myNextTarget != null)
            {
                myNextTarget.ActivateMeAsTarget();
                Destroy(myDebugLine);
            }

            aPlayer.gameObject.GetComponent<Fuel>().AddFuel(myFuelToAdd);
            aPlayer.gameObject.GetComponent<SpeedBoost>().ActivateSpeedBoost(mySpeedBoost);

           // aPlayer.gameObject.GetComponent<FOVAnimator>().ZoomFov(75);

            StageManager.ourInstance.OnPickedUpBlock();
            Destroy(gameObject);
        }
    }

    private void ActivateMeAsTarget()
    {
        // Make me collectible
        GetComponent<Collider>().enabled = true;

        // Make me glow        

        // Activate emission on material
        myMaterial.EnableKeyword("_EMISSION");
                
        // Set alpha to 1
        Color color = myMaterial.color;
        color.a = 1f;
        myMaterial.color = color;


        Behaviour halo = (Behaviour)myHalo.GetComponent("Halo");
        halo.enabled = true;
    }

    private void SetMaterialTransparent(Material aMaterial)
    {
        // Set alpha
        Color color = aMaterial.color;
        color.a = 0.25f;
        aMaterial.color = color;

        //https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html
        aMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        aMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        aMaterial.SetInt("_ZWrite", 0);
        aMaterial.DisableKeyword("_ALPHATEST_ON");
        aMaterial.DisableKeyword("_ALPHABLEND_ON");
        aMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        aMaterial.renderQueue = 3000;
    }

    
}

