using UnityEngine;

[System.Serializable]
public struct StageData
{
    public static StageData ourInvalid => new StageData { myPickedUpStarCount = 0, myStageDuration = 0.0f, myFinalScore = 0 };

    public bool myIsInvalid => myFinalScore <= 0;
    public bool myIsValid => myFinalScore > 0;

    public int myPickedUpStarCount;
    public float myStageDuration;

    public int myFinalScore;

    public string FormatDuration()
    {
        int minutes = Mathf.FloorToInt(myStageDuration / 60.0f);
        int seconds = Mathf.FloorToInt(myStageDuration) % 60;

        return $"{minutes:D2}:{seconds:D2}";
    }
}
