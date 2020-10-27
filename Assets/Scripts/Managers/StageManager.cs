using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager ourInstance;

    public static bool ourIsPaused => Mathf.Approximately(Time.timeScale, 0.0f);
    public static bool ourCanPause => !ourInstance.myIsPlayerDead && !ourInstance.myIsStageComplete;

    public bool myIsGoalEnabled => myPickedUpBlocksCount >= myBlockCount;

    public bool myIsStageComplete => !myStageData.myIsInvalid;

    public bool myIsPlayerDead { get; private set; } = false;
    public int myPickedUpBlocksCount { get; private set; } = 0;
    public float myStageStartTime { get; private set; } = 0.0f;

    public PickupScript myFirstBlock;
    public string myHomeworkText;

    [SerializeField]
    private int myBaseScore = 10000;

    [Header("Events")]
    public UnityEvent myOnPickedUpBlock = new UnityEvent();
    public UnityEvent myOnPickedUpStar = new UnityEvent();
    public UnityEvent myOnPauseStateChanged = new UnityEvent();
    public UnityEvent myOnPlayerDied = new UnityEvent();

    private int myBlockCount
    {
        get {
            Debug.Assert(myHomeworkText != null && myHomeworkText.Length > 0);

            return myHomeworkText.Replace(" ", string.Empty).Length;
        }
    }

    private StageData myStageData = StageData.ourInvalid;
    private StageData myHighscoreStageData = StageData.ourInvalid;

    private int myLastPickupAudioIndex = 0;

    public static void TogglePause()
    {
        if (ourIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public static void ResumeGame()
    {
        if (ourCanPause && ourIsPaused)
        {
            Time.timeScale = 1.0f;
            ourInstance.myOnPauseStateChanged?.Invoke();
        }
    }

    public static void PauseGame()
    {
        if (ourCanPause && !ourIsPaused)
        {
            Time.timeScale = 0.0f;
            ourInstance.myOnPauseStateChanged?.Invoke();
        }
    }

    public void OnPlayerDied()
    {
        if (!myIsPlayerDead)
        {
            myIsPlayerDead = true;
            myOnPlayerDied?.Invoke();
        }
    }

    public void OnStageComplete()
    {
        if (myIsStageComplete)
        {
            return;
        }

        myStageData.myStageDuration = Time.time - myStageStartTime;

        CalculateFinalScore();

        GameManager.ourInstance.SaveStageData(myStageData);

        GameManager.ourInstance.myAudioManager.PlayVoiceClip("stage_victory");

        UI.EndScreenMenu.ourInstance.DisplayEndScreen(true);
        FreezePlayer();
    }

    public void OnPickedUpBlock()
    {
        ++myPickedUpBlocksCount;

        myOnPickedUpBlock?.Invoke();

        int pickupAudioIndex;

        if (myLastPickupAudioIndex == 0)
        {
            pickupAudioIndex = Random.Range(1, 10);
        }
        else
        {
            pickupAudioIndex = Random.Range(1, 9);

            if (pickupAudioIndex >= myLastPickupAudioIndex)
            {
                ++pickupAudioIndex;
            }

            Debug.Assert(pickupAudioIndex != myLastPickupAudioIndex, "Same pickup sound should not be played back-to-back!");
        }

        myLastPickupAudioIndex = pickupAudioIndex;

        GameManager.ourInstance.myAudioManager.PlaySFXClip("picked_pickup");
        GameManager.ourInstance.myAudioManager.PlayVoiceClip($"point{pickupAudioIndex}");
    }

    public void OnPickedUpStar()
    {
        ++myStageData.myPickedUpStarCount;

        myOnPickedUpStar?.Invoke();
    }

    public StageData GetStageData() => myStageData;

    public void SetHighscoreStageData(StageData aHighscoreData)
    {
        myHighscoreStageData = aHighscoreData;
    }

    private void CalculateFinalScore()
    {        
        Debug.Assert(myStageData.myStageDuration > 0f, "myStageDuration is 0!");
        float score = myBaseScore / myStageData.myStageDuration;
        myStageData.myFinalScore = Mathf.FloorToInt(score);
    }

    private void FreezePlayer()
    {
        // Could cache references

        // Freeze the player
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerGameObject.GetComponent<PlaneController>().enabled = false;

        Rigidbody rigidbody = playerGameObject.GetComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // Freeze the camera
        Camera.main.transform.parent.GetComponent<CameraFollow>().enabled = false;
    }

    private void Awake()
    {
        Debug.Assert(ourInstance == null, "Multiple StageManagers loaded!");    
        ourInstance = this;

        SceneManager.LoadScene("UIBase", LoadSceneMode.Additive);
        SceneManager.LoadScene("PlayerUI", LoadSceneMode.Additive);
        SceneManager.LoadScene("PauseUI", LoadSceneMode.Additive);
        SceneManager.LoadScene("EndScreenUI", LoadSceneMode.Additive);
    }

    private void Start()
    {   
        if (myFirstBlock != null)
            myFirstBlock.SetActive(true);

        myStageStartTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
