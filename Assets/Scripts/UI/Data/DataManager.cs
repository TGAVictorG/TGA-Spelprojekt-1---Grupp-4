using UnityEngine;

public class DataManager : MonoBehaviour
{
#if UNITY_EDITOR

    private void Awake()
    {
        Debug.LogWarning("DataManager object can be removed from scenes! Has been migrated to non MonoBehaviour controlled by GameManager!");
    }

#endif
}
