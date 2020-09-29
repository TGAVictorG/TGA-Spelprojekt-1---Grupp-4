using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    public GameObject[] myLevelSelectButtons;
    public Image[] myStageUnlockGraphics;

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
        for (int i = 0; i < myLevelSelectButtons.Length; i++)
        {
            myLevelSelectButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = StageInformationRegistry.ourInstance.GetStageInformation(i).myStageDisplayName;

            if (StageInformationRegistry.ourInstance.IsStageUnlocked(i))
            {
                myStageUnlockGraphics[i].color = Color.green;
            }
            else
            {
                myStageUnlockGraphics[i].color = Color.red;
            }
        }
    }
}
