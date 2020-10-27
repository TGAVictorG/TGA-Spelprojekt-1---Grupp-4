using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private struct FadeData
    {
        public Renderer myRenderer;
        public Shader myOriginalShader;
    }

    public float myDistanceToTargetUp = 1;
    public float myDistanceToTargetBack = 2;

    public float myMoveSpeed = 6.5f;
    public float myRotationSpeed = 180.0f;
    [SerializeField] private float myBeforeStartRotationSpeed = 40.0f;
    [SerializeField] private float myBeforeStartMoveSpeed = 3.0f;
    [SerializeField] private float myBeforeStartRotationSpeedMultiplyerWhenTargetIsFound = 1.001f;

    [SerializeField] private float myObjectFadeSpeed = 5.0f;

    [SerializeField] private float myObjectRayDistanceBias = 0.02f;

    //[Header("Legacy (unused)")]
    //[SerializeField] private AnimationCurve myLookAtSpeedCurve;
    //[SerializeField] private AnimationCurve myMoveSpeedCurve;

    private Transform myTarget;

    private RaycastHit[] myAllHitsBuffer = new RaycastHit[16];
    private List<RaycastHit> myHitsBuffer = new List<RaycastHit>(16);
    private Dictionary<Collider, FadeData[]> myFadingElements = new Dictionary<Collider, FadeData[]>(16);
    private Dictionary<Collider, FadeData[]> myNotPresentElements = new Dictionary<Collider, FadeData[]>(16);

    private Shader myTransparentShader;
    private int myIgnoreCameraFadeLayer;

    private Vector3 myTargetPosition;
    private bool myIsBeforeStart = true;
    private float myCurrentRotationSpeed;
    private float myCurrentMoveSpeed;
    GameObject myTimer;

    private void PopulateHitBuffer(Vector3 anOrigin, Vector3 aDirection, float someMaxDistance)
    {
        // Clear old hits but keep capacity
        myHitsBuffer.Clear();

        int hits = Physics.RaycastNonAlloc(anOrigin, aDirection, myAllHitsBuffer, someMaxDistance);
        for (int i = 0; i < hits; ++i)
        {
            RaycastHit hit = myAllHitsBuffer[i];

            if (!hit.collider.isTrigger)
            {
                myHitsBuffer.Add(hit);
            }
        }
    }

    private Vector3 CalculateCameraTargetAndFade()
    {
        Vector3 playerForward = myTarget.forward;
        playerForward.y = 0.0f;
        playerForward.Normalize();

        myTargetPosition = myTarget.position - playerForward * myDistanceToTargetBack + Vector3.up * myDistanceToTargetUp;
        Vector3 playerToTarget = myTargetPosition - myTarget.position;
        float playerToTargetMagnitude = playerToTarget.magnitude;
        playerToTarget /= playerToTargetMagnitude;

        Vector3 playerToMe = transform.position - myTarget.position;
        float playerToMeMagnitude = playerToMe.magnitude;
        playerToMe /= playerToMeMagnitude;
        PopulateHitBuffer(myTarget.position, playerToMe, playerToMeMagnitude + myObjectRayDistanceBias);

        myNotPresentElements.Clear();
        foreach (KeyValuePair<Collider, FadeData[]> element in myFadingElements)
        {
            myNotPresentElements.Add(element.Key, element.Value);
        }

        float minDistance = -1.0f;
        for (int i = 0; i < myHitsBuffer.Count; ++i)
        {
            RaycastHit hit = myHitsBuffer[i];
            if (minDistance < 0.0f || hit.distance < minDistance)
            {
                minDistance = hit.distance;
            }

            if (hit.collider.gameObject.layer == myIgnoreCameraFadeLayer)
            {
                continue;
            }

            if (!myFadingElements.ContainsKey(hit.collider))
            {
                // Some GameObjects have the collider on a parent object and multiple renderers in children so we have to do it like this...
                Renderer[] hitRenderers = hit.transform.GetComponentsInChildren<Renderer>().Where(renderer => renderer.sharedMaterial.HasProperty("_Color")).ToArray();

                FadeData[] fadeData = new FadeData[hitRenderers.Length];
                for (int j = 0; j < fadeData.Length; ++j)
                {
                    Renderer renderer = hitRenderers[j];

                    fadeData[j] = new FadeData
                    {
                        myRenderer = renderer,
                        myOriginalShader = renderer.material.shader,
                    };

                    renderer.material.shader = myTransparentShader;
                }

                myFadingElements.Add(hit.collider, fadeData);
            }

            myNotPresentElements.Remove(hit.collider);

            foreach (FadeData fadeData in myFadingElements[hit.collider])
            {
                Renderer renderer = fadeData.myRenderer;

                Color fadeColor = renderer.material.color;
                fadeColor.a = Mathf.Max(fadeColor.a - myObjectFadeSpeed * Time.deltaTime, 0.0f);
                renderer.material.color = fadeColor;
            }
        }

        foreach (KeyValuePair<Collider, FadeData[]> element in myNotPresentElements)
        {
            for (int i = 0; i < element.Value.Length; ++i)
            {
                Renderer renderer = element.Value[i].myRenderer;
                renderer.material.shader = element.Value[i].myOriginalShader;

                Color opaqueColor = renderer.material.color;
                opaqueColor.a = 1.0f;

                renderer.material.color = opaqueColor;
            }

            myFadingElements.Remove(element.Key);
        }

        playerToTargetMagnitude = minDistance >= 0.0f ? Mathf.Min(playerToTargetMagnitude, minDistance) : playerToTargetMagnitude;

#if UNITY_EDITOR
        Debug.DrawRay(myTarget.position, playerToTarget * playerToTargetMagnitude, Color.red);
        Debug.DrawRay(myTarget.position, transform.position - myTarget.position, Color.blue);
#endif

        return myTarget.position + playerToTarget * playerToTargetMagnitude;
    }

    private void UpdatePosition()
    {
        Vector3 target = CalculateCameraTargetAndFade();

        transform.position = Vector3.MoveTowards(transform.position, target, myCurrentMoveSpeed * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        if (myIsBeforeStart)
        {
            if (Vector3.Dot((myTarget.position - transform.position).normalized, transform.forward) >= 0.9f)
            {
                myCurrentRotationSpeed *= myBeforeStartRotationSpeedMultiplyerWhenTargetIsFound;
            }

        }


        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(myTarget.position - transform.position), myCurrentRotationSpeed * Time.deltaTime);

    }

    private void CameraBeforeStart()
    {



        if (myTargetPosition == transform.position && myIsBeforeStart)
        {
            myCurrentMoveSpeed = myMoveSpeed;
            myIsBeforeStart = false;
            myCurrentRotationSpeed = myRotationSpeed;
            StageManager.ourInstance.ResetStageTime();
            myTarget.GetComponent<PlaneController>().enabled = true; 
            myTimer.GetComponent<TimerUI>().enabled = true;

        }
    }

    private void Awake()
    {
        myTransparentShader = Shader.Find("Transparent/Diffuse");
        myIgnoreCameraFadeLayer = LayerMask.NameToLayer("IgnoreCameraFade");
    }

    private void Start()
    {
        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(playerGo != null, "Could not find GameObject with Player tag, ensure the player is in the scene and is tagged Player!");

        myTarget = playerGo.transform;

        myCurrentRotationSpeed = myBeforeStartRotationSpeed;
        myCurrentMoveSpeed = myBeforeStartMoveSpeed;

        myTarget.GetComponent<PlaneController>().enabled = false;

        myTimer = GameObject.FindGameObjectWithTag("Timer");

        myTimer.GetComponent<TimerUI>().enabled = false;

        //transform.position = CalculateCameraTargetAndFade();
        //transform.LookAt(myTarget.position);
    }

    private void Update()
    {
        CameraBeforeStart();

        if(!StageManager.ourInstance.myIsPlayerDead)
        {
            UpdatePosition();
        }

        UpdateRotation();
    }
}
