using Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager ourInstance;

    public AudioManager myAudioManager { get; private set; }
    public StageInformationRegistry myStageInformationRegistry { get; private set; }

    [SerializeField]
    private GameObject myAudioManagerPrefab;

    [SerializeField]
    private GameObject myStageInformationRegistryPrefab;

    private AudioSource myCurrentMusicSource;

    private int myCurrentStageIndex = -1;

    public void SaveStageData(StageData aStageData)
    {
        Debug.Assert(myCurrentStageIndex >= 0, "Trying to save stage data on invalid stage!");
        myStageInformationRegistry.UpdateStageData(myCurrentStageIndex, aStageData);
    }

    #region Transitions

    public bool HasNextStage()
    {
        return myCurrentStageIndex >= 0 && (myCurrentStageIndex + 1) < myStageInformationRegistry.myStageCount;
    }

    public void TransitionNextStage()
    {
        if (HasNextStage())
        {
            TransitionToStage(myCurrentStageIndex + 1);
        }
        else
        {
            TransitionToMainMenu();
        }
    }

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

        PlayMusic("music_lvl1");
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

        StageManager.ourInstance.myOnPlayerDied.AddListener(StopMusic);

        PlayMusic($"music_lvl{aStageIndex + 1}", 0.25f);
    }

    private void PlayMusic(string anAudioName, float someVolume = 0.165f, bool aShouldRestart = false)
    {
        if (myCurrentMusicSource != null)
        {
            if (myCurrentMusicSource.clip == myAudioManager.GetAudioClip(anAudioName) && !aShouldRestart)
            {
                myCurrentMusicSource.volume = someVolume;
                return;
            }

            myAudioManager.Stop(myCurrentMusicSource);
            myCurrentMusicSource = null;
        }

        myCurrentMusicSource = myAudioManager.PlayMusicClip(anAudioName, someVolume: someVolume, aShouldLoop: true);
    }

    private void StopMusic()
    {
        if (myCurrentMusicSource != null)
        {
            myAudioManager.Stop(myCurrentMusicSource);
            myCurrentMusicSource = null;
        }
    }

    private void Start()
    {
        PlayMusic("music_lvl1");
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

        myAudioManager = Instantiate(myAudioManagerPrefab).GetComponent<AudioManager>();
        DontDestroyOnLoad(myAudioManager.gameObject);
        Debug.Assert(myAudioManager != null, "AudioManager not found on prefab!");

        myStageInformationRegistry = Instantiate(myStageInformationRegistryPrefab).GetComponent<StageInformationRegistry>();
        DontDestroyOnLoad(myStageInformationRegistry.gameObject);
        Debug.Assert(myStageInformationRegistry != null, "StageInformationRegistry not found on prefab!");
    }
}
