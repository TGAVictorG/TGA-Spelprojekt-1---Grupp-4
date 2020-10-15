using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    //-------------------------------------------------------------------------
    public class EndScreenMenu : MonoBehaviour
    {
        public static EndScreenMenu ourInstance;

        #region Private Serializable Fields

        [SerializeField] private Image myPopupImage;

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
            }
#endif

            ourInstance = this;

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

            myPopupImage.sprite = anIsVictory ? myWinSprite : myLossSprite;

            gameObject.SetActive(true);
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