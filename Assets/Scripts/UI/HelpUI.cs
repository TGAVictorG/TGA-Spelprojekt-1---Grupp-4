using UnityEngine;

namespace UI
{
    //-------------------------------------------------------------------------
    public class HelpUI : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("Universal Screen that holds all screens")]
        [SerializeField] private GameObject myUniversalScreen = null;

        [Header("Buttons")]
        [SerializeField] private GameObject myPreviousButton = null;
        [SerializeField] private GameObject myNextButton = null;
        [SerializeField] private GameObject myHelpButton = null;
        
        [Header("Tutorial Screens")]
        [SerializeField] private GameObject[] myTutorialScreens = null;
        
        #endregion
        
        
        #region Private Fields

        private int myCurrentIndex = 0;
        private bool myIsTutorialComplete = false;

        private bool myIsHelpEnabled = false;
		
        #endregion
        

        #region Unity Methods
        
        //-------------------------------------------------
        private void Start()
        {
            // Hide all screens
            if (myTutorialScreens.Length > 0)
            {
                foreach (GameObject screen in myTutorialScreens)
                {
                    screen.SetActive(false);
                }
            }
            
            // Hide Universal Screen
            myUniversalScreen.SetActive(false);

            if (myCurrentIndex == 0)
            {
                myPreviousButton.SetActive(false);
            }
        }

        #endregion


        #region Public Methods

        //-------------------------------------------------
        public void OnHelpButtonClicked()
        {
            myIsHelpEnabled = !myIsHelpEnabled;
            myUniversalScreen.SetActive(myIsHelpEnabled);
            myHelpButton.SetActive(!myIsHelpEnabled);

            if (myCurrentIndex == 0 && myIsHelpEnabled)
            {
                myTutorialScreens[myCurrentIndex].SetActive(true);
            }
        }
        
        //-------------------------------------------------
        public void OnNextButtonClicked()
        {
            myTutorialScreens[myCurrentIndex].SetActive(false);
            ++myCurrentIndex;
            myTutorialScreens[myCurrentIndex].SetActive(true);

            if (myCurrentIndex == myTutorialScreens.Length - 1)
            {
                myNextButton.SetActive(false);
            }

            if (myCurrentIndex == 1)
            {
                myPreviousButton.SetActive(true);
            }
        }
        
        //-------------------------------------------------
        public void OnPreviousButtonClicked()
        {
            myTutorialScreens[myCurrentIndex].SetActive(false);
            --myCurrentIndex;
            myTutorialScreens[myCurrentIndex].SetActive(true);
            
            if (myCurrentIndex == 0)
            {
                myPreviousButton.SetActive(false);
            }

            if (myCurrentIndex == myTutorialScreens.Length - 2)
            {
                myNextButton.SetActive(true);
            }
        }
        
        //-------------------------------------------------
        public void OnCloseButtonClicked()
        {
            OnHelpButtonClicked();
        }

        #endregion
    }
}