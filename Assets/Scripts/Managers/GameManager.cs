using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager ourInstance;

    public void CompleteStage(int aStageIndex, StageData aStageData)
    {
        StageInformationRegistry.ourInstance.UpdateStageData(aStageIndex, aStageData);
        TransitionToMainMenu();
    }

    #region Transitions

    public void TransitionToStage(int aStageIndex)
    {
        if (!StageInformationRegistry.ourInstance.IsStageUnlocked(aStageIndex))
        {
            return;
        }

        // TODO: Fade effect?
        SceneManager.LoadScene(StageInformationRegistry.ourInstance.GetStageInformation(aStageIndex).myStageSceneName, LoadSceneMode.Single);
        OnStageBegin(aStageIndex);
    }

    public void TransitionToMainMenu()
    {
        // TODO: Fade effect?
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    #endregion

    private void OnStageBegin(int aStageIndex)
    {
        Debug.Assert(StageManager.ourInstance != null, "StageManager not found in loaded stage!");

        if (StageInformationRegistry.ourInstance.HasValidStageData(aStageIndex))
        {
            StageManager.ourInstance.SetHighscoreStageData(StageInformationRegistry.ourInstance.GetStageData(aStageIndex));
        }
    }

    private void Awake()
    {
        Debug.Assert(ourInstance == null, "Multiple GameManagers loaded!");

        DontDestroyOnLoad(gameObject);
        ourInstance = this;
    }
}
