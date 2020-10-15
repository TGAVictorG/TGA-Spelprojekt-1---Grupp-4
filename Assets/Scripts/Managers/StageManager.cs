using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager ourInstance;

    public bool myIsGoalEnabled => myPickedUpBlocksCount >= myBlockCount;

    public bool myIsStageComplete => !myStageData.myIsInvalid;

    public int myPickedUpBlocksCount { get; private set; } = 0;
    public float myStageStartTime { get; private set; } = 0.0f;

    public PickupScript myFirstBlock;
    public string myHomeworkText;

    [SerializeField]
    private int myBaseScore = 10000;

    [Header("Events")]
    public UnityEvent myOnPickedUpBlock = new UnityEvent();
    public UnityEvent myOnPickedUpStar = new UnityEvent();

    private int myBlockCount
    {
        get {
            Debug.Assert(myHomeworkText != null && myHomeworkText.Length > 0);

            return myHomeworkText.Replace(" ", string.Empty).Length;
        }
    }

    private StageData myStageData = StageData.ourInvalid;
    private StageData myHighscoreStageData = StageData.ourInvalid;

    public void OnStageComplete()
    {
        if (myIsStageComplete)
        {
            return;
        }

        myStageData.myStageDuration = Time.time - myStageStartTime;

        CalculateFinalScore();

        GameManager.ourInstance.SaveStageData(myStageData);

        UI.EndScreenMenu.ourInstance.DisplayEndScreen(true);
        FreezePlayer();
    }

    public void OnPickedUpBlock()
    {
        ++myPickedUpBlocksCount;

        myOnPickedUpBlock?.Invoke();
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
        Camera.main.GetComponent<CameraFollow>().enabled = false;
    }

    private void Awake()
    {
        Debug.Assert(ourInstance == null, "Multiple StageManagers loaded!");    
        ourInstance = this;

        SceneManager.LoadScene("UIBase", LoadSceneMode.Additive);
        SceneManager.LoadScene("EndScreenScene", LoadSceneMode.Additive);
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
            PauseGame();

            // TODO: Show UI
        }
    }

    private static void PauseGame()
    {
        if (Mathf.Approximately(Time.timeScale, 1.0f))
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
