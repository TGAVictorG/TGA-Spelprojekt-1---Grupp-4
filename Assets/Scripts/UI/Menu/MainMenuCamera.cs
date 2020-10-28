using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCamera : MonoBehaviour
{
    private static bool myHasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (myHasStarted == false)
        {
            myHasStarted = true;
            SceneManager.LoadScene(10, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 5);
    }
}
