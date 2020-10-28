using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCamera : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(10, LoadSceneMode.Additive);
    }
}
