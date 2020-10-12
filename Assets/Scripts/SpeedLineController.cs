using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLineController : MonoBehaviour
{
    [SerializeField] TrailRenderer[] myTrailRenderers;
    [SerializeField] bool myEmitAtStartState;

    private void Start()
    {
        for (int i = 0; i < myTrailRenderers.Length; i++)
        {
            myTrailRenderers[i].emitting = myEmitAtStartState;
        }
    }

    public void Activate()
    {
        StartCoroutine(ActivateSpeedLines());
    }

    private IEnumerator ActivateSpeedLines()
    {
        float timeToLive = 1.0f;
        SpeedLineController trailRenderers = GetComponent<SpeedLineController>();
        SetSpeedLineState(true);

        while (timeToLive > 0)
        {
            timeToLive -= Time.deltaTime;
            yield return null;
        }

        SetSpeedLineState(false);
    }


    private void SetSpeedLineState(bool aState)
    {
        for (int i = 0; i < myTrailRenderers.Length; i++)
        {
            myTrailRenderers[i].emitting = aState;
        }
    }
}
