using UnityEngine;
using UnityEngine.Events;

public class StageInformationRegistry : MonoBehaviour
{
    public class StageDataWrapper
    {
        public StageData[] myStageData;
    }

    public event UnityAction myOnStageDataUpdated;

    public int myStageCount => myStageInformation.Length;

    public StageInformation[] myStageInformation;

    private StageDataWrapper myStageDataWrapper;

    public void UpdateStageData(int aStageIndex, StageData aStageData)
    {
        Debug.Assert(aStageIndex >= 0 && aStageIndex < myStageCount, "Tried fetching data from out-of-bounds stage index!");

        bool shouldReplace;
        if (myStageDataWrapper.myStageData[aStageIndex].myIsValid)
        {
            shouldReplace = myStageDataWrapper.myStageData[aStageIndex].myFinalScore < aStageData.myFinalScore;
        }
        else
        {
            shouldReplace = true;
        }

        if (shouldReplace)
        {
            myStageDataWrapper.myStageData[aStageIndex] = aStageData;
            SaveStageData();

            myOnStageDataUpdated?.Invoke();
        }
    }

    public StageData GetStageData(int aStageIndex)
    {
        Debug.Assert(aStageIndex >= 0 && aStageIndex < myStageCount, "Tried fetching data from out-of-bounds stage index!");

        return myStageDataWrapper.myStageData[aStageIndex];
    }

    public bool HasValidStageData(int aStageIndex)
    {
        return GetStageData(aStageIndex).myIsValid;
    }

    public StageInformation GetStageInformation(int aStageIndex)
    {
        Debug.Assert(aStageIndex >= 0 && aStageIndex < myStageCount, "Tried fetching data from out-of-bounds stage index!");

        return myStageInformation[aStageIndex];
    }

    public bool IsStageUnlocked(int aStageIndex)
    {
        if (GetStageInformation(aStageIndex).myIsForceUnlocked)
        {
            return true;
        }

        if (aStageIndex == 0)
        {
            return true; // First level should always be unlocked
        }

        // Check if the previous stage has completion data, if so then the next stage should be unlocked!
        return myStageDataWrapper.myStageData[aStageIndex - 1].myIsValid;
    }

    public void LoadStageData()
    {
        myStageDataWrapper = CreateDataSerializer().Load() ?? myStageDataWrapper;
    }

    public void SaveStageData()
    {
        CreateDataSerializer().Save(myStageDataWrapper);
    }

    public void ClearData()
    {
        CreateDataSerializer().Delete();
        ResetStageData();
    }

    private void ResetStageData()
    {
        myStageDataWrapper = new StageDataWrapper
        {
            myStageData = new StageData[myStageCount]
        };
    }

    private DataSerializer<StageDataWrapper> CreateDataSerializer() => new DataSerializer<StageDataWrapper>("stage_data.sav");

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            ClearData();
        }
    }
#endif

    private void Awake()
    {
        Debug.Assert(GameManager.ourInstance.myStageInformationRegistry == null, "StageInformationRegistry spawned when GameManager already owns one!");

        ResetStageData();
        LoadStageData();
    }
}
