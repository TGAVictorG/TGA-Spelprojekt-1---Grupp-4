using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupScript : MonoBehaviour
{

    [SerializeField] private float myRotatingSpeed = 50;
    [SerializeField] public PickupScript myNextTarget = null;
    [SerializeField] private float myFuelToAdd = 2.0f;
    [SerializeField] private float mySpeedBoost = 2.0f;
    [Header("Will boost player to a minimum speed")]
    [SerializeField] private float myBoostMinimumSpeed = 4.0f;
    [SerializeField] public bool myIsCheckpoint = false;
    [SerializeField] public Vector3 myRespawnDirection;

    private GameObject myHalo;
    private GameObject myHaloCheckpoint;
    private Material myMaterial;
    private Material myMaterialCheckpoint;
    private Color myOriginalColor;
    private float myEmissionIntensity = 1.0f;

    //Visar en cylinder till nästa pickup. Endast i Runtime.
    GameObject myDebugLine = null;
    [SerializeField] private bool myDebugLineIsVisable;
    [Tooltip("Can collect this pickup in any order.")]
    [SerializeField] private bool myDebugIsCollectible = true;    

    void Awake()
    {
        myHalo = transform.GetChild(0).gameObject;
        myHaloCheckpoint = transform.GetChild(1).gameObject;
        myMaterial = gameObject.GetComponent<Renderer>().material;
        myMaterialCheckpoint = myHaloCheckpoint.GetComponent<Renderer>().material;
        SetMaterialTransparent(myMaterial);
        myOriginalColor = myMaterial.color;

        Scene scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case "Level_1":
                myEmissionIntensity = 0.4f;
                break;
            case "Level_2":
                myEmissionIntensity = 0.4f;
                break;
            case "Level_3":
                myEmissionIntensity = 0.35f;
                break;
        }
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

                if (myIsCheckpoint)
                {
                    StageManager.ourInstance.myOnResetAtRespawn.RemoveAllListeners();
                    Debug.Log("Clearing all listeners on myOnResetBlocksAfterCheckpoint");
                }
                else if (StageManager.ourInstance.myCurrentCheckpoint != null)
                {
                    StageManager.ourInstance.myOnResetAtRespawn.AddListener(RestoreAsNotPickedUp);
                    Debug.Log("Adding a listener to myOnResetBlocksAfterCheckpoint");
                }
            }

            aPlayer.gameObject.GetComponent<Fuel>().AddFuel(myFuelToAdd);
            aPlayer.gameObject.GetComponent<SpeedBoost>().ActivateSpeedBoost(mySpeedBoost, myBoostMinimumSpeed);

            // aPlayer.gameObject.GetComponent<FOVAnimator>().ZoomFov(75);

            // Update current checkpoint
            if (myIsCheckpoint)
            {
                StageManager.ourInstance.myCurrentCheckpoint = transform;            
            }
            StageManager.ourInstance.OnPickedUpBlock(myNextTarget?.transform);
            
            // Hide current pickup
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Behaviour halo = (Behaviour)myHalo.GetComponent("Halo");
            halo.enabled = false;
            if (myIsCheckpoint)
            {
                Behaviour haloCheckpoint = (Behaviour)myHaloCheckpoint.GetComponent("Halo");
                haloCheckpoint.enabled = false;
            }
        }
    }

    public void ActivateMeAsTarget()
    {
        // Make me collectible
        GetComponent<Collider>().enabled = true;

        // Make me glow:
        // Activate emission on material
        if (!myIsCheckpoint)
        {
            // Set alpha to 1
            Color color = myOriginalColor;
            color.a = 1f;
            myMaterial.color = color;
            myMaterial.SetColor("_EmissionColor", color * myEmissionIntensity);

            Behaviour halo = (Behaviour)myHalo.GetComponent("Halo");
            halo.enabled = true;            
        }
        else
        {
            Color color = myMaterialCheckpoint.color;
            myMaterial.color = color;
            myMaterial.SetColor("_EmissionColor", color * myEmissionIntensity);

            Behaviour haloCheckpoint = (Behaviour)myHaloCheckpoint.GetComponent("Halo");
            haloCheckpoint.enabled = true;
        }

        myMaterial.EnableKeyword("_EMISSION");

    }

    private void RestoreAsNotPickedUp()
    {
        StageManager.ourInstance.myOnResetAtRespawn.RemoveListener(RestoreAsNotPickedUp);
        StageManager.ourInstance.OnResetBlock(); // Handle counter and UI
        gameObject.GetComponent<MeshRenderer>().enabled = true;

        // Tempfix. Actually only needs to be executed once, loops the linked list for disabling the next highlighted.
        var currentTarget = myNextTarget;
        while (currentTarget != null)
        {
            currentTarget.DisableHighlight();
            currentTarget = currentTarget?.myNextTarget;
        }
        DisableHighlight(); // Disable self
    }

    private void DisableHighlight()
    {
        // Make me not collectible
        GetComponent<Collider>().enabled = false;

        // Make me unglow:        
        myMaterial.color = myOriginalColor; // Set alpha to original
        myMaterial.DisableKeyword("_EMISSION");
        Behaviour halo = (Behaviour)myHalo.GetComponent("Halo");
        halo.enabled = false;
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


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (myIsCheckpoint == false || myRespawnDirection == null || myRespawnDirection == Vector3.zero)
        {
            return;
        }
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.cyan;
        var p1 = transform.position;
        var p2 = transform.position + myRespawnDirection.normalized*3f;
        var thickness = 3;
        UnityEditor.Handles.DrawBezier(p1, p2, p1, p2, Color.cyan, null, thickness);
    }

#endif

}

