using UnityEngine;

namespace Managers
{
    public class OldAudioManager : MonoBehaviour
    {
#if UNITY_EDITOR

        private void Awake()
        {
            Debug.LogWarning("AudioManager object (OldAudioManager component) can be removed from scenes! Has been moved to AudioManager owned by GameManager!");
        }

#endif
    }
}