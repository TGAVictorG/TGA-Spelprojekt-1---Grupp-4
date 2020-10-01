using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    public static StageManager ourInstance;

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

    private StageData myStageData = StageData.ourInvalid;
    private StageData myHighscoreStageData = StageData.ourInvalid;

    public void OnStageComplete()
    {
        GameManager.ourInstance.TransitionToMainMenu();

        if (myIsStageComplete)
        {
            return;
        }

        myStageData.myStageDuration = Time.time - myStageStartTime;

        CalculateFinalScore();

        // TODO: Call UI to show win ui with myStageData & possibly myHighscoreStageData (previous highscore, if it isn't invalid)
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

    public void SetHighscoreStageData(StageData aHighscoreData)
    {
        myHighscoreStageData = aHighscoreData;
    }

    private void CalculateFinalScore()
    {        
        Debug.Assert(myStageData.myStageDuration > 0f, "myStageDuration is 0!");
        float score = myBaseScore / myStageData.myStageDuration;
        myStageData.myFinalScore = (int) Mathf.FloorToInt(score);
    }

    private void Awake()
    {
        Debug.Assert(ourInstance == null, "Multiple StageManagers loaded!");    
        ourInstance = this;
    }

    private void Start()
    {        
        myFirstBlock.SetActive(true);
        myStageStartTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private static void PauseGame()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
