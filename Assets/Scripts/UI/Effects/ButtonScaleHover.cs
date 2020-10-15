using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleHover : EventTrigger
{
    [SerializeField]
    private float myHoverScale = 1.1f;

    [SerializeField]
    private float myScaleSpeed = 1.0f;

    private Vector3 myAnimationTarget = Vector3.one;

    public override void OnDeselect(BaseEventData eventData)
    {
        StopEffect();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        StartEffect();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        StopEffect();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        StartEffect();
    }

    private void StartEffect()
    {
        myAnimationTarget = Vector3.one * myHoverScale;
    }

    private void StopEffect()
    {
        myAnimationTarget = Vector3.one;
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, myAnimationTarget, myScaleSpeed * Time.deltaTime);
    }
}
