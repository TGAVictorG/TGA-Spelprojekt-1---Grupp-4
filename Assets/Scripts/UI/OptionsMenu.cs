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

		private OptionsDataManager myOptionsDataManager => GameManager.ourInstance.myOptionsDataManager;

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
			myOptionsDataManager.Load();
			
			myFullScreenToggle.isOn = myOptionsDataManager.FullScreenMode;
			myVsyncToggle.isOn = myOptionsDataManager.VSync;
			
			bool foundResolution = false;
			for (int index = 0; index < myResolutions.Length; ++index)
			{
				if (myOptionsDataManager.Resolution.myWidth == myResolutions[index].myWidth &&
				    myOptionsDataManager.Resolution.myHeight == myResolutions[index].myHeight)
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
			Debug.Assert(myOptionsDataManager != null, "myOptionsDataManager should not be null!");

			myOptionsDataManager.Save();
		}

		//-------------------------------------------------
		private void LoadAudioSettings()
		{
			Debug.Assert(myOptionsDataManager != null, "myOptionsDataManager should not be null!");

			myOptionsDataManager.Load();

			myMasterVolumeSlider.value = myOptionsDataManager.MasterVolume;
			myMusicVolumeSlider.value = myOptionsDataManager.MusicVolume;
			mySFXVolumeSlider.value = myOptionsDataManager.SFXVolume;
			myVoiceVolumeSlider.value = myOptionsDataManager.VoiceVolume;
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
			
			myOptionsDataManager.Resolution = myResolutions[mySelectedResolution];
			myOptionsDataManager.FullScreenMode = myFullScreenToggle.isOn;
			myOptionsDataManager.VSync = QualitySettings.vSyncCount != 0;
			
			SaveSettings();

			//  Simulates fullscreen toggling during development
#if UNITY_EDITOR
			EditorWindow window = EditorWindow.focusedWindow;
			window.maximized = myOptionsDataManager.FullScreenMode;
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
			myOptionsDataManager.Reset();
			LoadAudioSettings();
		}

		//-------------------------------------------------
		public void OnMasterVolumeChanged(float aValue)
		{
			Debug.Assert(myOptionsDataManager != null, "myOptionsDataManager should not be null!");

			myOptionsDataManager.MasterVolume = aValue;
			myAudioMixer.SetFloat(name: "MasterVolume", value: myOptionsDataManager.MasterVolume);
			myMasterVolumeLabelText.text = $"{myOptionsDataManager.MasterVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnMusicVolumeChanged(float aValue)
		{
			Debug.Assert(myOptionsDataManager != null, "myOptionsDataManager should not be null!");

			myOptionsDataManager.MusicVolume = aValue;
			myAudioMixer.SetFloat(name: "MusicVolume", value: myOptionsDataManager.MusicVolume);
			myMusicVolumeLabelText.text = $"{myOptionsDataManager.MusicVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnSFXVolumeChanged(float aValue)
		{
			Debug.Assert(myOptionsDataManager != null, "myOptionsDataManager should not be null!");

			myOptionsDataManager.SFXVolume = aValue;
			myAudioMixer.SetFloat(name: "SFXVolume", value: myOptionsDataManager.SFXVolume);
			mySFXVolumeLabelText.text = $"{myOptionsDataManager.SFXVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnVoiceVolumeChanged(float aValue)
		{
			Debug.Assert(myOptionsDataManager != null, "myOptionsDataManager should not be null!");

			myOptionsDataManager.VoiceVolume = aValue;
			myAudioMixer.SetFloat(name: "VoiceVolume", value: myOptionsDataManager.VoiceVolume);
			myVoiceVolumeLabelText.text = $"{myOptionsDataManager.VoiceVolume + 80:F0}";
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