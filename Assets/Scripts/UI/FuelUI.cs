using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    public Image myFuelBar;

    private Fuel myPlayerFuel;

    private void Start()
    {
        myPlayerFuel = GameObject.FindGameObjectWithTag("Player").GetComponent<Fuel>();
        if (myPlayerFuel == null)
        {
            Debug.LogWarning("Could not find Fuel on Player!");
            enabled = false;
        }
    }

    private void Update()
    {
        myFuelBar.fillAmount = myPlayerFuel.GetCurrentFuelPercentage();
    }
}
