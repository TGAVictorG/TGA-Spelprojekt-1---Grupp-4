using TMPro;
using UI.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
	//-------------------------------------------------------------------------
	/// <summary>
	/// This allows for anyone to set custom resolutions through the Inspector
	/// </summary>
	[System.Serializable]
	public class Resolution
	{
		public int myWidth;
		public int myHeight;
	}
	
	//-------------------------------------------------------------------------
	public class OptionsMenu : MonoBehaviour
	{
		#region Private Serialized Fields
		
		[Header("Configuration")]
		[SerializeField] private DataManager myDataManager = null;
		[SerializeField] private AudioMixer myAudioMixer = null;

		[Header("Audio Options | Sliders")]
		[SerializeField] private Slider myMasterVolumeSlider = null;
		[SerializeField] private Slider myMusicVolumeSlider = null;
		[SerializeField] private Slider mySFXVolumeSlider = null;
		[SerializeField] private Slider myVoiceVolumeSlider = null;

		[Header("Audio Options | Labels")]
		[SerializeField] private TextMeshProUGUI myMasterVolumeLabelText = null;
		[SerializeField] private TextMeshProUGUI myMusicVolumeLabelText = null;
		[SerializeField] private TextMeshProUGUI mySFXVolumeLabelText = null;
		[SerializeField] private TextMeshProUGUI myVoiceVolumeLabelText = null;

		[Header("Video Options")]
		[SerializeField] private Toggle myFullScreenToggle = null;
		[SerializeField] private Toggle myVsyncToggle = null;
		[SerializeField] private TextMeshProUGUI myResolutionLabelText = null;
		[SerializeField] private Resolution[] myResolutions = null;

        #endregion

        #region Public Fields

        [Header("Events")]
		public UnityEvent myOnCloseRequest = new UnityEvent();

        #endregion


        #region Private Fields

        private int mySelectedResolution = 0;
		
		#endregion
		
		
		#region Unity Methods

		//-------------------------------------------------
		private void Start()
		{
#if UNITY_EDITOR
			ValidateComponents();
#endif
			LoadVideoSettings();
			LoadAudioSettings();
		}
		
		#endregion


		#region Private Methods
		
		//-------------------------------------------------
		private void ValidateComponents()
		{
			// DataManager
			if (!myDataManager)
			{
				Debug.LogError("DataManager is NULL.");
			}
			
			// Sliders
			if (!myMasterVolumeSlider)
			{
				Debug.LogError("MasterVolume Slider is NULL.");
			}
			
			if (!myMusicVolumeSlider)
			{
				Debug.LogError("MusicVolume Slider is NULL.");
			}
			
			if (!mySFXVolumeSlider)
			{
				Debug.LogError("SFXVolume Slider is NULL.");
			}
			
			if (!myVoiceVolumeSlider)
			{
				Debug.LogError("VoiceVolume Slider is NULL.");
			}
			
			// Texts
			if (!myMasterVolumeLabelText)
			{
				Debug.LogError("MasterVolume Text is NULL.");
			}
			
			if (!myMusicVolumeLabelText)
			{
				Debug.LogError("MusicVolume Text is NULL.");
			}
			
			if (!mySFXVolumeLabelText)
			{
				Debug.LogError("SFXVolume Text is NULL.");
			}
			
			if (!myVoiceVolumeLabelText)
			{
				Debug.LogError("VoiceVolume Text is NULL.");
			}
		}
		
		//-------------------------------------------------
		private void LoadVideoSettings()
		{
			myDataManager.Load();
			
			myFullScreenToggle.isOn = myDataManager.FullScreenMode;
			myVsyncToggle.isOn = myDataManager.VSync;
			
			bool foundResolution = false;
			for (int index = 0; index < myResolutions.Length; ++index)
			{
				if (myDataManager.Resolution.myWidth == myResolutions[index].myWidth &&
				    myDataManager.Resolution.myHeight == myResolutions[index].myHeight)
				{
					foundResolution = true;
					mySelectedResolution = index;
					UpdateResolutionLabel();
					break;
				}
			}

			if (!foundResolution)
			{
				myResolutionLabelText.text = $"{Screen.width} x {Screen.height}";
			}
			
			SetVideoSettings();
		}
		
		//-------------------------------------------------
		private void SaveSettings()
		{
			if (myDataManager)
			{
				myDataManager.Save();
			}
		}

		//-------------------------------------------------
		private void LoadAudioSettings()
		{
			if (!myDataManager) return;
			
			myDataManager.Load();

			myMasterVolumeSlider.value = myDataManager.MasterVolume;
			myMusicVolumeSlider.value = myDataManager.MusicVolume;
			mySFXVolumeSlider.value = myDataManager.SFXVolume;
			myVoiceVolumeSlider.value = myDataManager.VoiceVolume;
		}
		
		//-------------------------------------------------
		private void UpdateResolutionLabel()
		{
			myResolutionLabelText.text =
				$"{myResolutions[mySelectedResolution].myWidth} x {myResolutions[mySelectedResolution].myHeight}";
		}

		//-------------------------------------------------
		private void SetVideoSettings()
		{
			Screen.SetResolution(
				width: myResolutions[mySelectedResolution].myWidth,
				height: myResolutions[mySelectedResolution].myHeight,
				fullscreen: myFullScreenToggle.isOn);
			
			QualitySettings.vSyncCount = myVsyncToggle.isOn ? 1 : 0;
			
			myDataManager.Resolution = myResolutions[mySelectedResolution];
			myDataManager.FullScreenMode = myFullScreenToggle.isOn;
			myDataManager.VSync = QualitySettings.vSyncCount != 0;
			
			SaveSettings();

			//  Simulates fullscreen toggling during development
#if UNITY_EDITOR
			EditorWindow window = EditorWindow.focusedWindow;
			window.maximized = myDataManager.FullScreenMode;
#endif
		}
		
		#endregion
	

		#region Public Methods
		
		//------------------- GENERAL ---------------------
		//-------------------------------------------------
		public void OnOptionsCloseButton()
		{
			SaveSettings();
			myOnCloseRequest?.Invoke();
		}
		
		
	
		//--------------------- AUDIO ---------------------
		//-------------------------------------------------
		public void OnAudioResetButtonClicked()
		{
			myDataManager.Reset();
			LoadAudioSettings();
		}

		//-------------------------------------------------
		public void OnMasterVolumeChanged(float aValue)
		{
			if (!myDataManager) return;
			
			myDataManager.MasterVolume = aValue;
			myAudioMixer.SetFloat(name: "MasterVolume", value: myDataManager.MasterVolume);
			myMasterVolumeLabelText.text = $"{myDataManager.MasterVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnMusicVolumeChanged(float aValue)
		{
			if (!myDataManager) return;
			
			myDataManager.MusicVolume = aValue;
			myAudioMixer.SetFloat(name: "MusicVolume", value: myDataManager.MusicVolume);
			myMusicVolumeLabelText.text = $"{myDataManager.MusicVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnSFXVolumeChanged(float aValue)
		{
			if (!myDataManager) return;
			
			myDataManager.SFXVolume = aValue;
			myAudioMixer.SetFloat(name: "SFXVolume", value: myDataManager.SFXVolume);
			mySFXVolumeLabelText.text = $"{myDataManager.SFXVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnVoiceVolumeChanged(float aValue)
		{
			if (!myDataManager) return;
			
			myDataManager.VoiceVolume = aValue;
			myAudioMixer.SetFloat(name: "VoiceVolume", value: myDataManager.VoiceVolume);
			myVoiceVolumeLabelText.text = $"{myDataManager.VoiceVolume + 80:F0}";
		}

		//--------------------- VIDEO ---------------------
		//-------------------------------------------------
		public void OnApplyVideoChangesButtonClicked()
		{
			SetVideoSettings();
		}
		
		//-------------------------------------------------
		public void OnResolutionMoveLeftClicked()
		{
			if (mySelectedResolution > 0)
			{ 
				--mySelectedResolution;
				UpdateResolutionLabel();
			}
		}

		//-------------------------------------------------
		public void OnResolutionMoveRightClicked()
		{
			if (mySelectedResolution < myResolutions.Length - 1)
			{
				++mySelectedResolution;
				UpdateResolutionLabel();
			}
		}

		#endregion
	}
}