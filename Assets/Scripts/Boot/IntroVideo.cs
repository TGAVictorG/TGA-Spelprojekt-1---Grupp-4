using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    [SerializeField]
    private VideoClip myVideoClip;

    private void OnVideoEnded()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            VideoPlayer videoPlayer = gameObject.GetComponent<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
            }

            OnVideoEnded();
        }
    }

    private void Awake()
    {
        if (myVideoClip != null)
        {
            VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.clip = myVideoClip;
            videoPlayer.isLooping = false;

            videoPlayer.loopPointReached += _ => OnVideoEnded();

            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No intro video!");

            OnVideoEnded();
        }
    }
}
