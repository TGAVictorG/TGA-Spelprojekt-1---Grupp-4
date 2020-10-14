using TMPro;
using UI.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //-------------------------------------------------------------------------
    public class EndScreenMenu : MonoBehaviour
    {
        public static EndScreenMenu ourInstance;

        #region Private Serializable Fields

        [Header("Configuration")]
        [SerializeField] private GameObject myEndScreenMenu = null;
        [SerializeField] private DataManager myDataManager = null;
        
        [Header("Scene Settings")]
        [SerializeField] private string myGameScene = string.Empty;
        [SerializeField] private string myMainMenuScene = string.Empty;

        [Header("Background Settings")]
        [SerializeField] private Image myBackgroundImage = null;
        [SerializeField] private Color myBackgroundVictoryColor = Color.green;
        [SerializeField] private Color myBackgroundGameOverColor = Color.red;
        
        [Header("Text Settings")]
        [SerializeField] private TextMeshProUGUI myGameOutcomeText = null;
        [SerializeField] private string myVictoryString = "VICTORY!";
        [SerializeField] private string myGameOverString = "WASTED!";
        [SerializeField] private Color myVictoryStringColor = Color.green;
        [SerializeField] private Color myGameOverStringColor = Color.red;
        
        #endregion


        #region Private Fields

        private bool myIsGameCompleted = false;

        #endregion


        #region Unity Methods

        //-------------------------------------------------
        private void Awake()
        {
            Debug.Assert(ourInstance == null, "Multiple EndScreenMenus loaded!");

            ourInstance = this;

            myEndScreenMenu.SetActive(false);
        }

        //-------------------------------------------------
        private void Start()
        {
#if UNITY_EDITOR
            ValidateComponents();
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                DisplayEndScreen(false);
            }
        }

        #endregion


        #region Private Methods
        
        //-------------------------------------------------
        private void ValidateComponents()
        {
            if (myGameScene == string.Empty)
            {
                Debug.LogError("Game Scene name is EMPTY:");
            }
            
            if (myMainMenuScene == string.Empty)
            {
                Debug.LogError("Main Menu Scene name is EMPTY:");
            }
            
            if (!myBackgroundImage)
            {
                Debug.LogError("Background Image is NULL.");
            }

            if (!myGameOutcomeText)
            {
                Debug.LogError("GameOutcomeText is NULL.");
            }
        }
        
        //-------------------------------------------------
        private void ChangeBackground()
        {
            if (!myBackgroundImage) return;
            
            if (myIsGameCompleted)
            {
                myBackgroundImage.color = myBackgroundVictoryColor;
            }
            else
            {
                myBackgroundImage.color = myBackgroundGameOverColor;
            }
        }

        //-------------------------------------------------
        private void ChangeText()
        {
            if (!myGameOutcomeText) return;
            
            if (myIsGameCompleted)
            {
                myGameOutcomeText.text = myVictoryString;
                myGameOutcomeText.color = myVictoryStringColor;
            }
            else
            {
                myGameOutcomeText.text = myGameOverString;
                myGameOutcomeText.color = myGameOverStringColor;
            }
        }

        #endregion


        #region Public Methods
        
        //-------------------------------------------------
        // NOTE: This should be called when the game ends, through victory or loss.
        public void DisplayEndScreen(bool anIsVictory)
        {
            myEndScreenMenu.SetActive(true);

            myDataManager.Load();
            myIsGameCompleted = anIsVictory;

            ChangeBackground();
            ChangeText();
        }

        //-------------------------------------------------
        public void OnPlayAgainClicked()
        {
            GameManager.ourInstance.RestartCurrentStage();
        }

        //-------------------------------------------------
        public void OnMainMenuClicked()
        {
            GameManager.ourInstance.TransitionToMainMenu();
        }

        //-------------------------------------------------
        public void OnQuitGameClicked()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}