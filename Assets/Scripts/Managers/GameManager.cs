using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager ourInstance;

    // NOTE: Could be dumbed down to a simple fixed size array, this would require a constant for the amount of stages in the game, and this approach doesn't
    private Dictionary<int, StageData> myStageData = new Dictionary<int, StageData>();

    public void CompleteStage(int aStageIndex, StageData aStageData)
    {
        bool shouldReplace;
        if (myStageData.ContainsKey(aStageIndex))
        {
            shouldReplace = myStageData[aStageIndex].myFinalScore < aStageData.myFinalScore;
        }
        else
        {
            shouldReplace = true;
        }

        if (shouldReplace)
        {
            myStageData[aStageIndex] = aStageData;
            SaveStageData(aStageIndex);
        }

        TransitionToMainMenu();
    }

    public bool IsStagedUnlocked(int aStageIndex)
    {
        if(myStageData[aStageIndex].myIsInvalid)
        {
            return false;
        }
        else if(!myStageData[aStageIndex].myIsInvalid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Transitions

    public void TransitionToStage(int aStageIndex)
    {
        // TODO: Structure of stage scene names
        if(!myStageData[aStageIndex-1].myIsInvalid)
        {
            SceneManager.LoadScene($"Stage_{aStageIndex}", LoadSceneMode.Single);
        }

        OnStageBegin(aStageIndex);
    }

    public void TransitionToMainMenu()
    {
        // TODO
    }

    #endregion

    #region Saving & Loading

    private void SaveStageData(int aStageIndex = -1)
    {
        // TODO: Save data to persistent storage
    }

    private void LoadStageData()
    {
        // TODO: Load saved data from persistent storage
    }

    #endregion

    private void OnStageBegin(int aStageIndex)
    {
        Debug.Assert(StageManager.ourInstance != null, "StageManager not found in loaded stage!");

        if (myStageData.ContainsKey(aStageIndex))
        {
            StageManager.ourInstance.SetHighscoreStageData(myStageData[aStageIndex]);
        }
    }

    private void Awake()
    {
        Debug.Assert(ourInstance == null, "Multiple GameManagers loaded!");

        DontDestroyOnLoad(gameObject);
        ourInstance = this;
    }

    private void Start()
    {
        LoadStageData();
    }
}
