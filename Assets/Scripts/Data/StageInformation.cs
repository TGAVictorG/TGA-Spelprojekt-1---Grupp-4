using UnityEngine;

[CreateAssetMenu(fileName = "Stage Information", menuName = "Stage/Data/Stage Information")]
public class StageInformation : ScriptableObject
{
    public string myStageDisplayName;
    public string myStageSceneName;

    public bool myIsForceUnlocked;

    public Sprite myStageThumbnail;
}
