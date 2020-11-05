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
    [SerializeField] private float myShorterCountdownLength = 1f;
    [SerializeField] private float myFadeTime;

    private Color myInitialColor;

    public bool start = false;
    public bool restart = false;

    private void Awake()
    {
        myInitialColor = myCountdownText.color;
    }

    private void Update()
    {
        if (start)
        {
            StartCountdown();
            start = false;
        }
        else if (restart)
        {
            StartShortCountdown();
            myCountdownText.color = myInitialColor;
            restart = false;
        }
    }

    public void StartCountdown()
    {
        StopAllCoroutines();
        StartCoroutine(StartCountdownCoroutine(myCountdownLength));
    }

    public void StartShortCountdown()
    {
        StopAllCoroutines();
        StartCoroutine(StartCountdownCoroutine(myShorterCountdownLength));
    }


    public IEnumerator StartCountdownCoroutine(float countdownLength)
    {
        float myCounter = countdownLength;

        while (myCounter > 0)
        {
            myCounter = Mathf.Clamp(myCounter - Time.deltaTime, 0, countdownLength);
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

        while (t > 0)
        {
            t = Mathf.Clamp(t - Time.deltaTime / myFadeTime, 0, 1);

            Color current = myCountdownText.color;
            current.a = t;

            myCountdownText.color = current;
            yield return null;
        }
    }
}
