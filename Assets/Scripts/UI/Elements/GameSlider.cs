using UnityEngine;
using UnityEngine.UI;

public class GameSlider : Slider
{
    [SerializeField]
    private RectTransform myFillArea;

    [SerializeField]
    private float myInsetLeft = 15.0f;

    [SerializeField]
    private float myInsetRight = 15.0f;

    private void OnValueChanged(float aValue)
    {
        float t = 1.0f - aValue;
        float width = (myFillArea.parent as RectTransform).rect.width - myInsetLeft - myInsetRight;

        myFillArea.offsetMax = new Vector2(-myInsetRight - width * t, myFillArea.offsetMax.y);
    }

    protected override void Set(float input, bool sendCallback = true)
    {
        base.Set(input, sendCallback);

        OnValueChanged(normalizedValue);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnValueChanged(normalizedValue);
    }
}
