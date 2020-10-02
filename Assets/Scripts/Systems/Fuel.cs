using System;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public bool myAllowFuelDepletion { get; set; } = true;
    public float myCurrentFuel { get; private set; }
    public bool myFuelIsEmpty { get; private set; }

    [Tooltip("In fuel / second")]
    [SerializeField] private float myFuelDepletionSpeed = 15.0f;
    [SerializeField] private float myMaxFuel = 100.0f;

    private void Start()
    {
        SetFuel(myMaxFuel);
    }

    private void Update()
    {
        if (myAllowFuelDepletion)
        {
            AddFuel(-myFuelDepletionSpeed * Time.deltaTime);
        }
    }

    private void SetFuel(float anAmount)
    {
        myCurrentFuel = Mathf.Clamp(anAmount, 0.0f, myMaxFuel);
        myFuelIsEmpty = myCurrentFuel <= 0 ? true : false;
    }

    public void AddFuel(float anAmount)
    {
        SetFuel(myCurrentFuel + anAmount);
    }

    public float GetCurrentFuelPercentage()
    {
        return myCurrentFuel / myMaxFuel;
    }

}
