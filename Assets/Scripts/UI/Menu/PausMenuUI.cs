using UnityEngine;

public class PausMenuUI : MonoBehaviour
{
    public void OnResumeClicked()
    {
        StageManager.ResumeGame();
    }

    public void OnOptionsClicked()
    {
        // TODO
    }

    public void OnMainMenuClicked()
    {
        StageManager.ResumeGame();
        GameManager.ourInstance.TransitionToMainMenu();
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    private void OnPauseStateChanged()
    {
        gameObject.SetActive(StageManager.ourIsPaused);
    }

    private void Start()
    {
        StageManager.ourInstance.myOnPauseStateChanged.AddListener(OnPauseStateChanged);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StageManager.ourInstance.myOnPauseStateChanged.RemoveListener(OnPauseStateChanged);
    }
}
