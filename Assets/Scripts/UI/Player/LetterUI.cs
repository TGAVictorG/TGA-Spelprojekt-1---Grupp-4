using UnityEngine;

public class LetterUI : MonoBehaviour
{
    public LetterLayout myLetterLayout;

    private void OnPickedUpBlock()
    {
        myLetterLayout.IncrementCurrentLetter();
    }

    void Start()
    {
        StageManager.ourInstance.myOnPickedUpBlock.AddListener(OnPickedUpBlock);
    }

    private void OnDestroy()
    {
        StageManager.ourInstance.myOnPickedUpBlock.RemoveListener(OnPickedUpBlock);
    }
}
