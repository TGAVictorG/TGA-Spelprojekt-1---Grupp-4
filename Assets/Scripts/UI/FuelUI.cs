using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    public Image myFuelBar;
    public Fuel myPlayerFuel;

    private void Update()
    {
        myFuelBar.fillAmount = myPlayerFuel.GetCurrentFuelPercentage();
    }
}
