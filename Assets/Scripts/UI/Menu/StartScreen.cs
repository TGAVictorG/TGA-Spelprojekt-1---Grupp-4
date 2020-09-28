using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject myMainMenu;

    //[SerializeField]
    //private GameObject myStartScreenGUI;


    private void Awake()
    {
        myMainMenu.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            myMainMenu.SetActive(true);
            //myStartScreenGUI.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
