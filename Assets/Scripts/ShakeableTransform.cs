using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableTransform : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("The frequency of the noise used in the shake if no other values get specified")]
    public float myDefaultFrequency = 10;
    [Tooltip("The speed of the recovery of the shake if no other value get specified")]
    public float myDefaultRecoverySpeed = 1.5f;
    [Tooltip("How hard the shake should be if no other value get specified")]
    public float myDefaultTraumaExponent = 2;
    [Tooltip("The maximum amount of rotation the shake can inflict in each axis if nothing else gets specified")]
    public Vector3 myDefaultMaximumAngularShake = Vector3.one * 2;

    [Header("Debug Settings")]
    [SerializeField]
    private bool myIsDebugging = false;

    private float myCurrentFrequency;
    private float myTrauma;
    private float myCurrentRecoverySpeed;
    private float myCurrentTraumaExponent;

    private Vector3 myCurrentMaximumAngularShake;

    private float mySeed;

    // Start is called before the first frame update
    private void Awake()
    {
        mySeed = Random.value;
        ResetToDefaults();
    }

    // Update is called once per frame
    void Update()
    {

        if(myTrauma != 0)
        {

            float shake = Mathf.Pow(myTrauma, myCurrentTraumaExponent);

            transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x + (myCurrentMaximumAngularShake.x * ((Mathf.PerlinNoise(mySeed + 3, Time.time * myCurrentFrequency)) * 2) - 1), transform.localRotation.y + myCurrentMaximumAngularShake.y * (Mathf.PerlinNoise(mySeed + 4, Time.time * myCurrentFrequency) * 2 - 1), transform.localRotation.z + myCurrentMaximumAngularShake.z * (Mathf.PerlinNoise(mySeed + 5, Time.time * myCurrentFrequency) * 2 - 1)) * shake);

            myTrauma = Mathf.Clamp01(myTrauma - myCurrentRecoverySpeed * Time.deltaTime);
            if(myTrauma == 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                ResetToDefaults();
            }
        }

    }

    void ResetToDefaults()
    {
        myCurrentFrequency = myDefaultFrequency;
        myCurrentRecoverySpeed = myDefaultRecoverySpeed;
        myCurrentMaximumAngularShake = myDefaultMaximumAngularShake;

        if(myIsDebugging == true)
        {
            Debug.Log("Reset default shake settings");
        }
    }

    public void ShakeCamera()
    {
        myTrauma = 1;
    }

    public void ShakeCamera(float aFrequency, float aRecoveryTime)
    {
        myCurrentFrequency = aFrequency;
        myCurrentRecoverySpeed = aRecoveryTime;

        myTrauma = 1;
    }

    public void ShakeCamera(float aFrequency, float aRecoverySpeed, float aTraumaExponent)
    {
        myCurrentFrequency = aFrequency;
        myCurrentRecoverySpeed = aRecoverySpeed;
        myCurrentTraumaExponent = aTraumaExponent;

        myTrauma = 1;
    }

    public void ShakeCamera(float aFrequency, float aRecoverySpeed, float aTraumaExponent, Vector3 aMaxRotationVector)
    {
        myCurrentFrequency = aFrequency;
        myCurrentRecoverySpeed = aRecoverySpeed;
        myCurrentTraumaExponent = aTraumaExponent;
        myCurrentMaximumAngularShake = aMaxRotationVector;

        myTrauma = 1;
    }

    public void ShakeCamera(float aFrequency, float aRecoverySpeed, Vector3 aMaxRotationVector)
    {
        myCurrentFrequency = aFrequency;
        myCurrentRecoverySpeed = aRecoverySpeed;
        myCurrentMaximumAngularShake = aMaxRotationVector;

        myTrauma = 1;
    }

    public void ShakeCamera(float aFrequency)
    {
        myCurrentFrequency = aFrequency;

        myTrauma = 1;
    }
}
