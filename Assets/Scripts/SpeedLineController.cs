using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLineController : MonoBehaviour
{
    [SerializeField] TrailRenderer[] myTrailRenderers;
    [SerializeField] bool myEmitAtStartState;
    private ParticleSystem mySpeedLineFX;

    private void Start()
    {
        for (int i = 0; i < myTrailRenderers.Length; i++)
        {
            myTrailRenderers[i].emitting = myEmitAtStartState;
        }
        mySpeedLineFX = Camera.main.transform.parent.GetComponentInChildren<ParticleSystem>();
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
        mySpeedLineFX.Play();
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
