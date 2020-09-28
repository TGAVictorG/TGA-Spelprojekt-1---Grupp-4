using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{

    public Image[] myStageUnlockGraphics;

    GameManager myGameManager = GameManager.ourInstance;

    public void LoadLevel(int aSceneBuildIndex)
    {
        myGameManager.TransitionToStage(aSceneBuildIndex);
    }

    private void Update()
    {
        for (int i = 0; i < myStageUnlockGraphics.Length; i++)
        {
            if(myGameManager.IsStagedUnlocked(i) == true)
            {
               // myStageUnlockGraphics[i].
            }
        }
    }


}
