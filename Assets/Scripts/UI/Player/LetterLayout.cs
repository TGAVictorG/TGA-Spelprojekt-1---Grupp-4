using UnityEngine;
using UnityEngine.UI;

public class LetterLayout : LayoutGroup
{
    [SerializeField] private float myLetterSpacing = 48.0f;
    [SerializeField] private float myLetterSize = 100.0f;

    [SerializeField] private float myAnimationSpeed = 5.0f;
    [SerializeField] private float myCurrentScaleAdd = 0.3f;

    [SerializeField] private Sprite[] myOrderedLetters;

    private int myCurrentLetterIndex;
    private float myCurrentLetterAnimationIndex;

    private DrivenRectTransformTracker myTracker;

    public void IncrementCurrentLetter()
    {
        SetCurrentLetterIndex(myCurrentLetterIndex + 1, true);
    }

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();
    }

    public override void SetLayoutHorizontal()
    {
        myTracker.Clear();

        float width = (myLetterSize + myLetterSpacing) * 3 - myLetterSpacing;
        myTracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

        float centerX = (rectTransform.rect.width * 0.5f) - myLetterSize * 0.5f;

        for(int i = 0; i < rectTransform.childCount; ++i)
        {
            RectTransform childRectTransform = rectTransform.GetChild(i) as RectTransform;

            float offset = i - myCurrentLetterAnimationIndex;
            float alpha = 1.0f - Mathf.Clamp((Mathf.Abs(offset) - 1.0f) * 1.0f, 0.0f, 1.0f);

            Image childImage = childRectTransform.GetComponent<Image>();
            Color color = childImage.color;
            color.a = alpha;

            childImage.color = color;

            SetChildAlongAxis(childRectTransform, 0, centerX + offset * (childRectTransform.rect.width + myLetterSpacing), myLetterSize);
            childRectTransform.localScale = Vector3.one * (1.0f + myCurrentScaleAdd * (1.0f - Mathf.Abs(offset)));
        }
    }

    public override void SetLayoutVertical()
    {
        myTracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, myLetterSize);

        for (int i = 0; i < rectChildren.Count; ++i)
        {
            SetChildAlongAxis(rectChildren[i], 1, 0.0f, myLetterSize);
        }
    }

    private void SetCurrentLetterIndex(int aLetterIndex, bool aShouldAnimate = false)
    {
        myCurrentLetterIndex = aLetterIndex;
        if (!aShouldAnimate)
        {
            myCurrentLetterAnimationIndex = aLetterIndex;
        }

        SetDirty();
    }

    private void CreateChildLetter(char aLetter)
    {
        GameObject gameObject = new GameObject("Letter " + aLetter, typeof(RectTransform));
        gameObject.transform.SetParent(transform);

        Image image = gameObject.AddComponent<Image>();
        image.sprite = myOrderedLetters[aLetter - 'a'];

        RectTransform rectTransform = gameObject.transform as RectTransform;

        rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1.0f);
    }

    private void GenerateLetters()
    {
        DestroyLetters();

        string letters = StageManager.ourInstance.myHomeworkText.Replace(" ", "").ToLower();
        for (int i = 0; i < letters.Length; ++i)
        {
            CreateChildLetter(letters[i]);
        }

        SetCurrentLetterIndex(0);
    }

    private void DestroyLetters()
    {
        for (int i = 0; i < rectTransform.childCount; ++i)
        {
            Destroy(rectTransform.GetChild(i).gameObject);
        }
    }

    protected void Update()
    {
        float prevAnimationIndex = myCurrentLetterAnimationIndex;
        myCurrentLetterAnimationIndex = Mathf.MoveTowards(myCurrentLetterAnimationIndex, myCurrentLetterIndex, myAnimationSpeed * Time.unscaledDeltaTime);

        if (!Mathf.Approximately(prevAnimationIndex, myCurrentLetterAnimationIndex))
        {
            SetDirty();
        }
    }

    protected override void Start()
    {
        base.Start();

        if (Application.isPlaying)
        {
            GenerateLetters();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        myTracker.Clear();
    }
}
