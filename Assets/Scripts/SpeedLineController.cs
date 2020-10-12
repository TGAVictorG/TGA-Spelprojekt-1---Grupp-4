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

    public void ActivateSpeedLines(bool aState)
    {
        for (int i = 0; i < myTrailRenderers.Length; i++)
        {
            myTrailRenderers[i].emitting = aState;
        }
    }
}
