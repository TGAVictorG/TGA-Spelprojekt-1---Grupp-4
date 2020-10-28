using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    public Image myFuelBar;
    public Image myLowFuelBar;
    public Image myFuelPickupEffectBar;
    [SerializeField] private float myLowFuelOscilationPeriod;
    [SerializeField] private float myFuelPickupEffectTime;
    [SerializeField] private float myFuelBarSpeed = 3.0f;
    private Fuel myPlayerFuel;
    private float myTimeSinceStart;

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
        else
        {
            myPlayerFuel.OnFuelPickup += FuelPickup;
        }

        myTimeSinceStart = (3 * myLowFuelOscilationPeriod) / 4;
    }


    private void Update()
    {
        myFuelBar.fillAmount = Mathf.Lerp(myFuelBar.fillAmount, myPlayerFuel.GetCurrentFuelPercentage(), Time.deltaTime * myFuelBarSpeed);
        myFuelPickupEffectBar.fillAmount = myPlayerFuel.GetCurrentFuelPercentage();

        if (myPlayerFuel.IsLowOnFuel())
        {
            myLowFuelBar.gameObject.SetActive(true);

            float fuelBarAlpha = (Mathf.Sin(((Mathf.PI * 2) / myLowFuelOscilationPeriod) * myTimeSinceStart) + 1) / 2f;
            myTimeSinceStart += Time.deltaTime;

            Color fuelBarColor = myLowFuelBar.color;
            fuelBarColor.a = fuelBarAlpha;
            myLowFuelBar.color = fuelBarColor;
        }
        else
        {
            myLowFuelBar.gameObject.SetActive(false);
        }
    }

    private void FuelPickup(float anAmount)
    {
        StartCoroutine(FuelPickupEffect());
    }

    private IEnumerator FuelPickupEffect()
    {
        float timer = 0;

        myFuelPickupEffectBar.gameObject.SetActive(true);

        while (timer < 1)
        {
            timer += Time.deltaTime / myFuelPickupEffectTime;
            timer = Mathf.Clamp(timer, 0, 1);

            Color barColor = myFuelPickupEffectBar.color;
            barColor.a = Mathf.Sin(timer * Mathf.PI);
            myFuelPickupEffectBar.color = barColor;

            yield return null;
        }

        myFuelPickupEffectBar.gameObject.SetActive(false);
    }
}
