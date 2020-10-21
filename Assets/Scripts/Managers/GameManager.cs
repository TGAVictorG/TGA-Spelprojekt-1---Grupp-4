using Managers;
using System.Collections;
using UI.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager ourInstance;

    public OptionsDataManager myOptionsDataManager { get; private set; }

    public AudioManager myAudioManager { get; private set; }
    public StageInformationRegistry myStageInformationRegistry { get; private set; }

    [SerializeField]
    private GameObject myAudioManagerPrefab;

    [SerializeField]
    private GameObject myStageInformationRegistryPrefab;

    private int myCurrentStageIndex = -1;

    public void SaveStageData(StageData aStageData)
    {
        Debug.Assert(myCurrentStageIndex >= 0, "Trying to save stage data on invalid stage!");
        myStageInformationRegistry.UpdateStageData(myCurrentStageIndex, aStageData);
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
        if (!myStageInformationRegistry.IsStageUnlocked(aStageIndex))
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

        SceneManager.LoadScene(myStageInformationRegistry.GetStageInformation(aStageIndex).myStageSceneName, LoadSceneMode.Single);

        // Wait for next frame when the scene is fully loaded and active
        yield return null;

        OnStageBegin(aStageIndex);
    }

    private void OnStageBegin(int aStageIndex)
    {
        myCurrentStageIndex = aStageIndex;

        Debug.Assert(StageManager.ourInstance != null, "StageManager not found in loaded stage!");

        if (myStageInformationRegistry.HasValidStageData(aStageIndex))
        {
            StageManager.ourInstance.SetHighscoreStageData(myStageInformationRegistry.GetStageData(aStageIndex));
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

        myOptionsDataManager = new OptionsDataManager();

        myAudioManager = Instantiate(myAudioManagerPrefab).GetComponent<AudioManager>();
        DontDestroyOnLoad(myAudioManager.gameObject);
        Debug.Assert(myAudioManager != null, "myAudioManager not found on prefab!");

        myStageInformationRegistry = Instantiate(myStageInformationRegistryPrefab).GetComponent<StageInformationRegistry>();
        DontDestroyOnLoad(myStageInformationRegistry.gameObject);
        Debug.Assert(myStageInformationRegistry != null, "myStageInformationRegistry not found on prefab!");
    }
}
