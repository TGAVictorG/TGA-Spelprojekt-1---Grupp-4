using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableTransform : MonoBehaviour
{

    public float myFrequency = 10f;
    public float myTrauma;
    public float myRecoverySpeed = 1.5f;

    public bool myIsDebugging = false;

    float myTraumaExponent = 2;

   // public Vector3 myMaximumTranslationShake = Vector3.one * 0.5f;
    public Vector3 myMaximumAngularShake = Vector3.one * 2;

    private float mySeed;

    // Start is called before the first frame update
    private void Awake()
    {
        mySeed = Random.value;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0) && myIsDebugging)
        {
            ShakeCamera(1);
        }


        float shake = Mathf.Pow(myTrauma, myTraumaExponent);

        transform.localRotation = Quaternion.Euler(new Vector3(myMaximumAngularShake.x * (Mathf.PerlinNoise(mySeed + 3, Time.time * myFrequency) * 2 - 1),myMaximumAngularShake.y * (Mathf.PerlinNoise(mySeed + 4, Time.time * myFrequency) * 2 - 1),myMaximumAngularShake.z * (Mathf.PerlinNoise(mySeed + 5, Time.time * myFrequency) * 2 - 1)) * shake);

        myTrauma = Mathf.Clamp01(myTrauma - myRecoverySpeed * Time.deltaTime);
    }

    public void ShakeCamera(float aShakeAmount)
    {
        myTrauma = Mathf.Clamp01(myTrauma+aShakeAmount);
    }
}
