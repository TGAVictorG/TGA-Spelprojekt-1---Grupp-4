using UnityEngine;
using UnityEngine.Video;

public class MainMenuBackdropVideo : MonoBehaviour
{
    [SerializeField]
    private VideoClip myVideoClip;

    void Start()
    {
        if (myVideoClip != null)
        {
            VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.aspectRatio = VideoAspectRatio.FitOutside;
            videoPlayer.clip = myVideoClip;
            videoPlayer.isLooping = true;

            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No backdrop video!");
        }
    }
}
