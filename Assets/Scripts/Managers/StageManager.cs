using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    public static StageManager ourInstance;

    public bool myIsStageComplete => !myStageData.myIsInvalid;

    public int myPickedUpBlocksCount { get; private set; } = 0;
    public float myStageStartTime { get; private set; } = 0.0f;

    public PickupScript myFirstBlock;
    public string myHomeworkText;

    [Header("Events")]
    public UnityEvent myOnPickedUpBlock = new UnityEvent();
    public UnityEvent myOnPickedUpStar = new UnityEvent();

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

        // TODO: Call UI to show win ui with myStageData & possibly myHighscoreStageData (previous highscore, if it isn't invalid)
    }

    public void OnPickedUpBlock()
    {
        ++myPickedUpBlocksCount;
        print("here");
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
        // TODO: Use myStageData to calculate the final value for myStageData.myFinalScore
        myStageData.myFinalScore = 1;
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
}
