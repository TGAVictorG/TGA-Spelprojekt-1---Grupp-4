using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    public bool myAllowFuelDepletion { get; set; } = true;

    public float myMaxFuel = 100.0f;

    [Tooltip("In fuel / second")]
    public float myFuelDepletionSpeed = 15.0f;

    private float myCurrentFuel;

    public void SetFuel(float anAmount)
    {
        myCurrentFuel = Mathf.Clamp(anAmount, 0.0f, myMaxFuel);
    }

    public void AddFuel(float anAmount)
    {
        SetFuel(myCurrentFuel + anAmount);
    }

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
}
