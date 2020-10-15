using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    public Image myFuelBar;

    private Fuel myPlayerFuel;

    private void Start()
    {
#if UNITY_EDITOR
        if (gameObject.scene.name != "PlayerUI")
        {
            Debug.LogWarning("Part of Player UI not in PlayerUI Scene! FuelUI not in Player UI Scene!");
            gameObject.SetActive(false);
        }
#endif
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
