using UnityEngine;
using UnityEngine.Events;

public class Fuel : MonoBehaviour
{
    public delegate void FuelPickup(float amount);
    public event FuelPickup OnFuelPickup;

    public bool myAllowFuelDepletion { get; set; } = true;
    public float myCurrentFuel { get; private set; }
    public bool myFuelIsEmpty { get; private set; }

    [Tooltip("In fuel / second")]
    [SerializeField] private float myFuelDepletionSpeed = 15.0f;
    [SerializeField] private float myMaxFuel = 100.0f;
    [SerializeField] private float myLowFuelPercentThreshold = .20f;

    private void Awake()
    {
        SetFuel(myMaxFuel);
    }

    private void Update()
    {
        if (myAllowFuelDepletion)
        {
            SetFuel(myCurrentFuel + (-myFuelDepletionSpeed * Time.deltaTime));
        }
    }

    private void SetFuel(float anAmount)
    {
        myCurrentFuel = Mathf.Clamp(anAmount, 0.0f, myMaxFuel);
        myFuelIsEmpty = myCurrentFuel <= 0 ? true : false;
    }

    public void AddFuel(float anAmount)
    {
        if (OnFuelPickup != null)
        {
            OnFuelPickup.Invoke(anAmount);
        }

        SetFuel(myCurrentFuel + anAmount);
    }

    public float GetCurrentFuelPercentage()
    {
        return myCurrentFuel / myMaxFuel;
    }

    public bool IsLowOnFuel()
    {
        return GetCurrentFuelPercentage() < myLowFuelPercentThreshold;
    }
}
