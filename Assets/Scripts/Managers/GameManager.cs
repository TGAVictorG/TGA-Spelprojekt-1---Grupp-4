using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager ourInstance;

    private int myCurrentStageIndex = -1;

    public void SaveStageData(StageData aStageData)
    {
        Debug.Assert(myCurrentStageIndex >= 0, "Trying to save stage data on invalid stage!");
        StageInformationRegistry.ourInstance.UpdateStageData(myCurrentStageIndex, aStageData);
    }

    #region Transitions

    public void RestartCurrentStage()
    {
        if (myCurrentStageIndex >= 0)
        {
            TransitionToStage(myCurrentStageIndex);
        }
        else
        {
            TransitionToMainMenu();
        }
    }

    public void TransitionToStage(int aStageIndex)
    {
        if (!StageInformationRegistry.ourInstance.IsStageUnlocked(aStageIndex))
        {
            return;
        }

        StartCoroutine(LoadStage(aStageIndex));
    }

    public void TransitionToMainMenu()
    {
        myCurrentStageIndex = -1;

        // TODO: Fade effect?
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    #endregion

    private IEnumerator LoadStage(int aStageIndex)
    {
        // TODO: Fade effect?

        SceneManager.LoadScene(StageInformationRegistry.ourInstance.GetStageInformation(aStageIndex).myStageSceneName, LoadSceneMode.Single);

        // Wait for next frame when the scene is fully loaded and active
        yield return null;

        OnStageBegin(aStageIndex);
    }

    private void OnStageBegin(int aStageIndex)
    {
        myCurrentStageIndex = aStageIndex;

        Debug.Assert(StageManager.ourInstance != null, "StageManager not found in loaded stage!");

        if (StageInformationRegistry.ourInstance.HasValidStageData(aStageIndex))
        {
            StageManager.ourInstance.SetHighscoreStageData(StageInformationRegistry.ourInstance.GetStageData(aStageIndex));
        }
    }

    private void Awake()
    {
        if (ourInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        ourInstance = this;
    }
}
