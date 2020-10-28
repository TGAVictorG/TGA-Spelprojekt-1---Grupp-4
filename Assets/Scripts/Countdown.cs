using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    public delegate void CountdownFinished();
    public event CountdownFinished OnCountdownFinished;

    [SerializeField] private Image myBorder;
    [SerializeField] private TextMeshProUGUI myCountdownText;
    [SerializeField] private float myCountdownLength;

    public bool start = false;
    private void Update()
    {
        if(start)
        {
            StartCountdown();
            start = false;
        }
    }

    public void StartCountdown()
    {        
        StartCoroutine(StartCountdownCoroutine());
    }

    public IEnumerator StartCountdownCoroutine()
    {
        float myCounter = myCountdownLength;

        while (myCounter > 0)
        {
            myCounter = Mathf.Clamp(myCounter - Time.deltaTime, 0, myCountdownLength);
            myCountdownText.text = "" + Mathf.Ceil(myCounter);
            myBorder.fillAmount = myCounter % 1;

            yield return null;
        }

        if(OnCountdownFinished != null)
        {
            OnCountdownFinished.Invoke();
        }

        StartCoroutine(FadeUICoroutine());
    }
    public IEnumerator FadeUICoroutine()
    {
        float t = 1;

        while (t >= 0)
        {
            t -= Time.deltaTime / 0.2f;

            Color current = myCountdownText.color;
            current.a = t;

            myCountdownText.color = current;
            yield return null;
        }
    }
}
