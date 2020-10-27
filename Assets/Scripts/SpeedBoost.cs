using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedBoost : MonoBehaviour
{
    public delegate void SpeedBoostEvent(float aValue1, float aValue2);
    public event SpeedBoostEvent myOnSpeedBoost;

    private void Start()
    {
    }

    public void ActivateSpeedBoost(float aBoostAmount, float aMinimumSpeed)
    {
        if(myOnSpeedBoost != null)
        {
            myOnSpeedBoost.Invoke(aBoostAmount, aMinimumSpeed);
        }
    }
}
