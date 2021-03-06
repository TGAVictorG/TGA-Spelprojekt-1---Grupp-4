﻿namespace UI.Data
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
        public Resolution myResolution;

        // Game Data
        public bool myUseInvertedFlightControls;

        //-------------------------------------------------
        public SaveData()
        {
            SetDefaultAudioSettings();
            SetDefaultVideoSettings();
            SetDefaultGameSettings();
        }

        //-------------------------------------------------
        public void SetDefaultAudioSettings()
        {
            myMasterVolume = 100.0f;
            myMusicVolume = 100.0f;
            mySFXVolume = 100.0f;
            myVoiceVolume = 100.0f;
        }
        
        //-------------------------------------------------
        public void SetDefaultVideoSettings()
        {
            myIsFullScreenOn = true;
            myIsVsyncOn = false;

            myResolution = GraphicsManager.ourInstance.GetBestResolution();
        }

        public void SetDefaultGameSettings()
        {
            myUseInvertedFlightControls = true;
        }
    }
}