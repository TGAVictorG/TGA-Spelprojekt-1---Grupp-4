using UnityEngine;

public class AudioPlayProxy : MonoBehaviour
{
    public void PlaySFX(string anAudioName)
    {
        GameManager.ourInstance.myAudioManager.PlaySFXClip(anAudioName);
    }

    public void PlayUIClick()
    {
        PlaySFX("button_click");
    }

    public void PlayUIHover()
    {
        PlaySFX("button_hover");
    }
}
