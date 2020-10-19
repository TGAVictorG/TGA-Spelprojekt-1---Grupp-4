using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoSequenceUI : MonoBehaviour
{
    [SerializeField]
    private Sprite[] myLogoSequence;

    [SerializeField]
    private float myAnimateInDuration = 2.0f;
    [SerializeField]
    private float myFreezeDuration = 1.0f;
    [SerializeField]
    private float myAnimateOutDuration = 1.0f;

    [SerializeField]
    private float myInitialDelay = 0.5f;

    [Min(float.Epsilon)]
    [SerializeField]
    private float myStartScale = 0.7f;

    [SerializeField]
    private Image myImage;

    private AspectRatioFitter myAspectRatioFitter;

    private IEnumerator AnimateLogo(Sprite aLogo)
    {
        Color color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Vector3 startScale = Vector3.one * myStartScale;

        RectTransform rectTransform = myImage.rectTransform;

        myImage.sprite = aLogo;
        myImage.color = color;
        rectTransform.localScale = startScale;

        myAspectRatioFitter.aspectRatio = aLogo.rect.width / aLogo.rect.height;

        myImage.enabled = true;

        float time = 0.0f;

        do
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / myAnimateInDuration);

            color.a = Mathf.Lerp(0.0f, 1.0f, t);

            myImage.color = color;
            rectTransform.localScale = Vector3.Slerp(startScale, Vector3.one, t);

            yield return null;
        } while (time < myAnimateInDuration);

        yield return new WaitForSeconds(myFreezeDuration);

        time = 0.0f;

        do
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / myAnimateOutDuration);

            color.a = Mathf.Lerp(1.0f, 0.0f, t);

            myImage.color = color;

            yield return null;
        } while (time < myAnimateOutDuration);
    }

    private IEnumerator Start()
    {
        myAspectRatioFitter = myImage.GetComponent<AspectRatioFitter>();
        myImage.enabled = false;

        int currentIndex = 0;

        yield return new WaitForSeconds(myInitialDelay);

        while (currentIndex < myLogoSequence.Length)
        {
            yield return AnimateLogo(myLogoSequence[currentIndex++]);
        }

        SceneManager.LoadScene("Intro");
    }
}