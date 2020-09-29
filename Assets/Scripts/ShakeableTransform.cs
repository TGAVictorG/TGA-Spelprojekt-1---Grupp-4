using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableTransform : MonoBehaviour
{

    public float myFrequency = 10f;

    public Vector3 myMaximumTranslationShake = Vector3.one * 0.5f;
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
        transform.localPosition = new Vector3(myMaximumTranslationShake.x * Mathf.PerlinNoise(mySeed, Time.time * myFrequency) * 2 - 1, myMaximumTranslationShake.y * Mathf.PerlinNoise(mySeed + 1, Time.time * myFrequency) * 2 - 1, myMaximumTranslationShake.z * Mathf.PerlinNoise(mySeed + 2, Time.time * myFrequency) * 2 - 1) * 0.5f;

        transform.localRotation = Quaternion.Euler(new Vector3(myMaximumAngularShake.x * (Mathf.PerlinNoise(mySeed + 3, Time.time * myFrequency) * 2 - 1),myMaximumAngularShake.y * (Mathf.PerlinNoise(mySeed + 4, Time.time * myFrequency) * 2 - 1),myMaximumAngularShake.z * (Mathf.PerlinNoise(mySeed + 5, Time.time * myFrequency) * 2 - 1)));
    }
}
