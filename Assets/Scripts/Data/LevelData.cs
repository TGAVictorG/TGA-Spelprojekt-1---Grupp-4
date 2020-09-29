using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level/Data/Level Data")]
public class LevelData : ScriptableObject
{
    public int mySceneIndex;

    public string myLevelName;

    public bool myIsUnlocked;

    public Sprite myLevelThumbnail;


}
