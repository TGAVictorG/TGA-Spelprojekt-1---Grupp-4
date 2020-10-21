using UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private LevelSelectUI myLevelSelectUI;
    
    [SerializeField]
    private OptionsMenu myOptionsUI;

    [SerializeField]
    private GameObject myMainUI;

    [SerializeField]
    private GameObject myCreditUI;

    public void OpenOptions()
    {
        myMainUI.SetActive(false);
        myOptionsUI.gameObject.SetActive(true);
    }

    public void CloseOptions()
    {
        if (!myOptionsUI.gameObject.activeSelf)
        {
            return;
        }

        myMainUI.SetActive(true);
        myOptionsUI.gameObject.SetActive(false);
    }

    public void OpenLevelSelect()
    {
        myMainUI.SetActive(false);
        myLevelSelectUI.gameObject.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        if (!myLevelSelectUI.gameObject.activeSelf)
        {
            return;
        }

        myMainUI.SetActive(true);
        myLevelSelectUI.gameObject.SetActive(false);
    }

    public void OpenCredits()
    {
        myMainUI.SetActive(false);
        myCreditUI.SetActive(true);
    }

    public void CloseCredits()
    {
        if (!myCreditUI.activeSelf)
        {
            return;
        }

        myMainUI.SetActive(true);
        myCreditUI.SetActive(false);
    }

    public void Start()
    {
        myOptionsUI.myOnCloseRequest.AddListener(CloseOptions);
    }

    public void OnDestroy()
    {
        myOptionsUI.myOnCloseRequest.RemoveListener(CloseOptions);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
