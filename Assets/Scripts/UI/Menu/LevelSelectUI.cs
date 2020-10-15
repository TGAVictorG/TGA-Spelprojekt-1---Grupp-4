using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [System.Serializable]
    private struct LevelUIData
    {
#pragma warning disable 0649
        public Button myButton;
        public Image myLockImage;

        public Text myLevelNameText;
        public Text myLevelRecordText;
#pragma warning restore 0649
    }

    [SerializeField]
    private LevelUIData[] myLevelUIData;

    public void LoadLevel(int aStageIndex)
    {
        GameManager.ourInstance.TransitionToStage(aStageIndex);
    }

    private void Start()
    {
        StageInformationRegistry.ourInstance.myOnStageDataUpdated += UpdateLevelSelectionGraphics;
        UpdateLevelSelectionGraphics();
    }

    private void OnDestroy()
    {
        StageInformationRegistry.ourInstance.myOnStageDataUpdated -= UpdateLevelSelectionGraphics;
    }

    public void UpdateLevelSelectionGraphics()
    {
        StageInformationRegistry stageInformationRegistry = StageInformationRegistry.ourInstance;

        for (int i = 0; i < myLevelUIData.Length; ++i)
        {
            LevelUIData levelUIData = myLevelUIData[i];

            levelUIData.myLevelNameText.text = stageInformationRegistry.GetStageInformation(i).myStageDisplayName;

            if (stageInformationRegistry.HasValidStageData(i))
            {
                float duration = stageInformationRegistry.GetStageData(i).myStageDuration;

                int minutes = Mathf.FloorToInt(duration / 60.0f);
                int seconds = Mathf.FloorToInt(duration) % 60;

                levelUIData.myLevelRecordText.text = $"{minutes:D2}:{seconds:D2}";
            }
            else
            {
                levelUIData.myLevelRecordText.text = string.Empty;
            }

            bool isUnlocked = stageInformationRegistry.IsStageUnlocked(i);

            levelUIData.myButton.enabled = isUnlocked;
            levelUIData.myLockImage.enabled = !isUnlocked;
            levelUIData.myLevelNameText.enabled = isUnlocked;
            levelUIData.myLevelRecordText.enabled = isUnlocked;
        }
    }
}
