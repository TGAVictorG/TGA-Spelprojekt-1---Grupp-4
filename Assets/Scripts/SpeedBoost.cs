using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventFloat : UnityEvent<float>{}

public class SpeedBoost : MonoBehaviour
{
    public delegate void SpeedBoostEvent(float value);
    public event SpeedBoostEvent myOnSpeedBoost;

    private void Start()
    {
    }

    public void ActivateSpeedBoost(float aBoostAmount)
    {
        if(myOnSpeedBoost != null)
        {
            myOnSpeedBoost.Invoke(aBoostAmount);
        }
    }
}
