using UnityEngine;

namespace UI.Data
{
    //-------------------------------------------------------------------------
    public class OptionsDataManager
    {
        public static OptionsDataManager ourInstance
        {
            get
            {
                if (ourUniqueInstance == null)
                {
                    ourUniqueInstance = new OptionsDataManager();
                    ourUniqueInstance.Load();
                }

                return ourUniqueInstance;
            }
        }

        #region Private Fields

        private static OptionsDataManager ourUniqueInstance;

        private SaveData _mySaveData = new SaveData();
        private JsonConverter _myJsonConverter = new JsonConverter();

        #endregion

        #region Properties
        
        #region Audio

        public float MasterVolume
        {
            get => _mySaveData.myMasterVolume;
            set => _mySaveData.myMasterVolume = value;
        }

        public float MusicVolume
        {
            get => _mySaveData.myMusicVolume;
            set => _mySaveData.myMusicVolume = value;
        }

        public float SFXVolume
        {
            get => _mySaveData.mySFXVolume;
            set => _mySaveData.mySFXVolume = value;
        }

        public float VoiceVolume
        {
            get => _mySaveData.myVoiceVolume;
            set => _mySaveData.myVoiceVolume = value;
        }

        #endregion


        #region Video

        public bool FullScreenMode
        {
            get => _mySaveData.myIsFullScreenOn;
            set => _mySaveData.myIsFullScreenOn = value;
        }

        public bool VSync
        {
            get => _mySaveData.myIsVsyncOn;
            set => _mySaveData.myIsVsyncOn = value;
        }

        public Resolution Resolution
        {
            get => _mySaveData.myResolution;
            set => _mySaveData.myResolution = value;
        }

        #endregion
        
        #endregion
        
        #region Public Methods

        //-------------------------------------------------
        public void Save()
        {
            _myJsonConverter.Save(_mySaveData);
            
#if UNITY_EDITOR
            Debug.Log("Data saved to disk.");
#endif
        }

        //-------------------------------------------------
        public void Load()
        {
#if UNITY_EDITOR
            Debug.Log(_myJsonConverter.Load(_mySaveData)
                ? "Save Data loaded successfully."
                : "Error: unable to load save data.");
#else
            _myJsonConverter.Load(_mySaveData);
#endif
        }
        
        //-------------------------------------------------
        public void ResetAndSave()
        {
            _mySaveData.SetDefaultAudioSettings();

            Save();
        }

        #endregion
    }
}