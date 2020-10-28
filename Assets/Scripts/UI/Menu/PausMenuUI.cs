using UI;
using UnityEngine;

public class PausMenuUI : MonoBehaviour
{
    [SerializeField]
    private OptionsMenu myOptionsUI;

    [SerializeField]
    private GameObject myPopup;

    public void OnResumeClicked()
    {
        StageManager.ResumeGame();
    }

    public void OnOptionsClicked()
    {
        myPopup.SetActive(false);
        myOptionsUI.gameObject.SetActive(true);
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

    private void CloseOptions()
    {
        if (!myOptionsUI.gameObject.activeSelf)
        {
            return;
        }

        myPopup.SetActive(true);
        myOptionsUI.gameObject.SetActive(false);
    }

    private void OnPauseStateChanged()
    {
        myOptionsUI.gameObject.SetActive(false);
        myPopup.SetActive(StageManager.ourIsPaused);

        gameObject.SetActive(StageManager.ourIsPaused);
    }

    private void Start()
    {
        StageManager.ourInstance.myOnPauseStateChanged.AddListener(OnPauseStateChanged);
        myOptionsUI.myOnCloseRequest.AddListener(CloseOptions);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        myOptionsUI.myOnCloseRequest.RemoveListener(CloseOptions);
        StageManager.ourInstance.myOnPauseStateChanged.RemoveListener(OnPauseStateChanged);
    }
}
