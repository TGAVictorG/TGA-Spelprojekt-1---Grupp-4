namespace UI.Data
{
    //-------------------------------------------------------------------------
    [System.Serializable]
    public class SaveData
    {
        // Audio Data
        public float myMasterVolume;
        public float myMusicVolume;
        public float mySFXVolume;
        public float myVoiceVolume;
        
        // Video Data
        public bool myIsFullScreenOn;
        public bool myIsVsyncOn;
        public Resolution myResolution = null;

        //-------------------------------------------------
        public SaveData()
        {
            SetDefaultAudioSettings();

            myIsFullScreenOn = false;
            myIsVsyncOn = false;
            myResolution = new Resolution {myWidth = 1920, myHeight = 1080};
        }

        //-------------------------------------------------
        public void SetDefaultAudioSettings()
        {
            myMasterVolume = 0.0f;
            myMusicVolume = 0.0f;
            mySFXVolume = 0.0f;
            myVoiceVolume = 0.0f;
        }
    }
}