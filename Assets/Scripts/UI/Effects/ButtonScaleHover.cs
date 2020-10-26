using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleHover : EventTrigger
{
    [SerializeField]
    private AudioManager.AudioType myAudioType = AudioManager.AudioType.SFX;

    [SerializeField]
    private string myClickAudioName = "button_click";

    [SerializeField]
    private float myHoverScale = 1.1f;

    [SerializeField]
    private float myScaleSpeed = 1.0f;

    [SerializeField]
    private bool myEnableSounds = true;

    private Vector3 myAnimationTarget = Vector3.one;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (myEnableSounds)
        {
            GameManager.ourInstance.myAudioManager.Play2D(GameManager.ourInstance.myAudioManager.GetAudioClip(myClickAudioName), myAudioType);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        StartEffect();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        StopEffect();
    }

    private void StartEffect(bool anAllowAudio = true)
    {
        myAnimationTarget = Vector3.one * myHoverScale;

        if (anAllowAudio && myEnableSounds)
        {
            GameManager.ourInstance.myAudioManager.PlaySFXClip("button_hover");
        }
    }

    private void StopEffect()
    {
        myAnimationTarget = Vector3.one;
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, myAnimationTarget, myScaleSpeed * Time.unscaledDeltaTime);
    }

    private void OnDisable()
    {
        StopEffect();
        transform.localScale = myAnimationTarget;
    }
}
