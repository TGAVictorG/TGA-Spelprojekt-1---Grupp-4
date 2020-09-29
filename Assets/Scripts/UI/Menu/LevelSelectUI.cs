using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{

    public GameObject[] levelSelectButtons;
    public Image[] myStageUnlockGraphics;

    public LevelData[] myLevelData;

    public void LoadLevel(int aSceneBuildIndex)
    {
        GameManager.ourInstance.TransitionToStage(aSceneBuildIndex);
    }

    private void Start()
    {
        GameManager.ourInstance.onLevelUnlocked += UpdateLevelSelectionGraphics;
        UpdateLevelSelectionGraphics();
    }

    public void UpdateLevelSelectionGraphics()
    {
        for (int i = 0; i < levelSelectButtons.Length; i++)
        {
            levelSelectButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = myLevelData[i].myLevelName;

            if (myLevelData[i].myIsUnlocked)
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
