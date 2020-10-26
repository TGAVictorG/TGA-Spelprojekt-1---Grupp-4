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
            SetDefaultVideoSettings();
        }

        //-------------------------------------------------
        public void SetDefaultAudioSettings()
        {
            myMasterVolume = 80.0f;
            myMusicVolume = 80.0f;
            mySFXVolume = 80.0f;
            myVoiceVolume = 80.0f;
        }
        
        //-------------------------------------------------
        public void SetDefaultVideoSettings()
        {
            myIsFullScreenOn = true;
            myIsVsyncOn = false;
            myResolution = new Resolution {myWidth = 1920, myHeight = 1080};
        }
    }
}