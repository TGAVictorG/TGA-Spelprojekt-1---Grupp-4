using UnityEngine;

public class LetterUI : MonoBehaviour
{
    public LetterLayout myLetterLayout;

    private void OnPickedUpBlock()
    {
        myLetterLayout.IncrementCurrentLetter();
    }

    private void OnResetBlock()
    {
        myLetterLayout.DecrementCurrentLetter();
    }

    void Start()
    {
        StageManager.ourInstance.myOnPickedUpBlock.AddListener(OnPickedUpBlock);
        StageManager.ourInstance.myOnResetBlock.AddListener(OnResetBlock);
    }

    private void OnDestroy()
    {
        StageManager.ourInstance.myOnPickedUpBlock.RemoveListener(OnPickedUpBlock);
    }
}
