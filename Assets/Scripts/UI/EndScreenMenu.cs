using UnityEditor;
using UnityEngine;

namespace UI
{
    //-------------------------------------------------------------------------
    public class EndScreenMenu : MonoBehaviour
    {
        public static EndScreenMenu ourInstance;

        #region Private Serializable Fields

        [SerializeField] private GameObject myDefeatPopup;
        [SerializeField] private GameObject myVictoryPopup;

        [Header("Victory References")]
        [SerializeField] private GameObject myContinueButton;
        [SerializeField] private TMPro.TextMeshProUGUI myHomeworkText;
        [SerializeField] private TMPro.TextMeshProUGUI myTimeText;

        [Header("Configuration")]
        [SerializeField] private Sprite myWinSprite;
        [SerializeField] private Sprite myLossSprite;

        #endregion

        #region Unity Methods

        //-------------------------------------------------
        private void Awake()
        {
            Debug.Assert(ourInstance == null, "Multiple EndScreenMenus loaded!");

#if UNITY_EDITOR
            if (gameObject.scene.name != "EndScreenUI")
            {
                Debug.LogWarning("EndScreenMenu in non EndScreenUI scene! Please remove EndScreenMenu UI from stage scenes!");
                Destroy(gameObject);
            }
#endif

            ourInstance = this;

            myDefeatPopup.SetActive(false);
            myVictoryPopup.SetActive(false);
            gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods
        
        //-------------------------------------------------
        // NOTE: This should be called when the game ends, through victory or loss.
        public void DisplayEndScreen(bool anIsVictory)
        {
            if (gameObject.activeSelf)
            {
                return;
            }

            if (anIsVictory)
            {
                Debug.Assert(StageManager.ourInstance.myIsStageComplete);

                myHomeworkText.text = StageManager.ourInstance.myHomeworkText;
                myTimeText.text = StageManager.ourInstance.GetStageData().FormatDuration();

                myContinueButton.SetActive(GameManager.ourInstance.HasNextStage());

                myVictoryPopup.SetActive(true);
            }
            else
            {
                myDefeatPopup.SetActive(true);
            }

            gameObject.SetActive(true);
        }

        public void OnContinueClicked()
        {
            GameManager.ourInstance.TransitionNextStage();
        }

        //-------------------------------------------------
        public void OnRestartClicked()
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