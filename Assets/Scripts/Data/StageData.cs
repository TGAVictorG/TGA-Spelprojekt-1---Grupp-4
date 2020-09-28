public struct StageData
{
    public static StageData ourInvalid => new StageData { myPickedUpStarCount = 0, myStageDuration = 0.0f, myFinalScore = 0 };

    public bool myIsInvalid => myFinalScore <= 0;

    public int myPickedUpStarCount;
    public float myStageDuration;

    public int myFinalScore;
}