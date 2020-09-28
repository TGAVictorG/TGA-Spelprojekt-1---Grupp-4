using TMPro;
using UI.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
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
		[SerializeField] private DataManager _myDataManager = null;
		[SerializeField] private AudioMixer _myAudioMixer = null;
		[SerializeField] private GameObject _myAudioOptionsScreen = null;

		[Header("Audio Options | Sliders")]
		[SerializeField] private Slider _myMasterVolumeSlider = null;
		[SerializeField] private Slider _myMusicVolumeSlider = null;
		[SerializeField] private Slider _mySFXVolumeSlider = null;
		[SerializeField] private Slider _myVoiceVolumeSlider = null;

		[Header("Audio Options | Labels")]
		[SerializeField] private TextMeshProUGUI _myMasterVolumeLabelText = null;
		[SerializeField] private TextMeshProUGUI _myMusicVolumeLabelText = null;
		[SerializeField] private TextMeshProUGUI _mySFXVolumeLabelText = null;
		[SerializeField] private TextMeshProUGUI _myVoiceVolumeLabelText = null;

		[Header("Video Options")]
		[SerializeField] private Toggle _myFullScreenToggle = null;
		[SerializeField] private Toggle _myVsyncToggle = null;
		[SerializeField] private TextMeshProUGUI _myResolutionLabelText = null;
		[SerializeField] private Resolution[] _myResolutions = null;
		
		#endregion


		#region Private Fields
		
		private int _mySelectedResolution = 0;
		
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
			if (!_myDataManager)
			{
				Debug.LogError("DataManager is NULL.");
			}
			
			// Sliders
			if (!_myMasterVolumeSlider)
			{
				Debug.LogError("MasterVolume Slider is NULL.");
			}
			
			if (!_myMusicVolumeSlider)
			{
				Debug.LogError("MusicVolume Slider is NULL.");
			}
			
			if (!_mySFXVolumeSlider)
			{
				Debug.LogError("SFXVolume Slider is NULL.");
			}
			
			if (!_myVoiceVolumeSlider)
			{
				Debug.LogError("VoiceVolume Slider is NULL.");
			}
			
			// Texts
			if (!_myMasterVolumeLabelText)
			{
				Debug.LogError("MasterVolume Text is NULL.");
			}
			
			if (!_myMusicVolumeLabelText)
			{
				Debug.LogError("MusicVolume Text is NULL.");
			}
			
			if (!_mySFXVolumeLabelText)
			{
				Debug.LogError("SFXVolume Text is NULL.");
			}
			
			if (!_myVoiceVolumeLabelText)
			{
				Debug.LogError("VoiceVolume Text is NULL.");
			}
		}
		
		//-------------------------------------------------
		private void LoadVideoSettings()
		{
			_myDataManager.Load();
			
			_myFullScreenToggle.isOn = _myDataManager.FullScreenMode;
			_myVsyncToggle.isOn = _myDataManager.VSync;
			
			bool foundResolution = false;
			for (int index = 0; index < _myResolutions.Length; ++index)
			{
				if (_myDataManager.Resolution.myWidth == _myResolutions[index].myWidth &&
				    _myDataManager.Resolution.myHeight == _myResolutions[index].myHeight)
				{
					foundResolution = true;
					_mySelectedResolution = index;
					UpdateResolutionLabel();
					break;
				}
			}

			if (!foundResolution)
			{
				_myResolutionLabelText.text = $"{Screen.width} x {Screen.height}";
			}
		}
		
		//-------------------------------------------------
		private void SaveSettings()
		{
			if (_myDataManager)
			{
				_myDataManager.Save();
			}
		}

		//-------------------------------------------------
		private void LoadAudioSettings()
		{
			if (!_myDataManager) return;
			
			_myDataManager.Load();

			_myMasterVolumeSlider.value = _myDataManager.MasterVolume;
			_myMusicVolumeSlider.value = _myDataManager.MusicVolume;
			_mySFXVolumeSlider.value = _myDataManager.SFXVolume;
			_myVoiceVolumeSlider.value = _myDataManager.VoiceVolume;
		}
		
		//-------------------------------------------------
		private void UpdateResolutionLabel()
		{
			_myResolutionLabelText.text =
				$"{_myResolutions[_mySelectedResolution].myWidth} x {_myResolutions[_mySelectedResolution].myHeight}";
		}

		#endregion
	

		#region Public Methods
	
		//--------------------- AUDIO ---------------------
		//-------------------------------------------------
		public void OnAudioCloseButtonClicked()
		{
			SaveSettings();
			_myAudioOptionsScreen.SetActive(false);
		}

		//-------------------------------------------------
		public void OnAudioResetButtonClicked()
		{
			_myDataManager.Reset();
			LoadAudioSettings();
		}

		//-------------------------------------------------
		public void OnMasterVolumeChanged(float aValue)
		{
			if (!_myDataManager) return;
			
			_myDataManager.MasterVolume = aValue;
			_myAudioMixer.SetFloat(name: "MasterVolume", value: _myDataManager.MasterVolume);
			_myMasterVolumeLabelText.text = $"{_myDataManager.MasterVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnMusicVolumeChanged(float aValue)
		{
			if (!_myDataManager) return;
			
			_myDataManager.MusicVolume = aValue;
			_myAudioMixer.SetFloat(name: "MusicVolume", value: _myDataManager.MusicVolume);
			_myMusicVolumeLabelText.text = $"{_myDataManager.MusicVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnSFXVolumeChanged(float aValue)
		{
			if (!_myDataManager) return;
			
			_myDataManager.SFXVolume = aValue;
			_myAudioMixer.SetFloat(name: "SFXVolume", value: _myDataManager.SFXVolume);
			_mySFXVolumeLabelText.text = $"{_myDataManager.SFXVolume + 80:F0}";
		}
	
		//-------------------------------------------------
		public void OnVoiceVolumeChanged(float aValue)
		{
			if (!_myDataManager) return;
			
			_myDataManager.VoiceVolume = aValue;
			_myAudioMixer.SetFloat(name: "VoiceVolume", value: _myDataManager.VoiceVolume);
			_myVoiceVolumeLabelText.text = $"{_myDataManager.VoiceVolume + 80:F0}";
		}

		//--------------------- VIDEO ---------------------
		//-------------------------------------------------
		public void OnApplyVideoChangesButtonClicked()
		{
			Screen.SetResolution(
				width: _myResolutions[_mySelectedResolution].myWidth,
				height: _myResolutions[_mySelectedResolution].myHeight,
				fullscreen: _myFullScreenToggle.isOn);
			
			QualitySettings.vSyncCount = _myVsyncToggle.isOn ? 1 : 0;
			
			_myDataManager.Resolution = _myResolutions[_mySelectedResolution];
			_myDataManager.FullScreenMode = _myFullScreenToggle.isOn;
			_myDataManager.VSync = QualitySettings.vSyncCount != 0;
			
			SaveSettings();

			//  Simulates fullscreen toggling during development
#if UNITY_EDITOR
			EditorWindow window = EditorWindow.focusedWindow;
			window.maximized = _myDataManager.FullScreenMode;
#endif
		}
		
		//-------------------------------------------------
		public void OnResolutionMoveLeftClicked()
		{
			if (_mySelectedResolution > 0)
			{ 
				--_mySelectedResolution;
				UpdateResolutionLabel();
			}
		}

		//-------------------------------------------------
		public void OnResolutionMoveRightClicked()
		{
			if (_mySelectedResolution < _myResolutions.Length - 1)
			{
				++_mySelectedResolution;
				UpdateResolutionLabel();
			}
		}

		#endregion
	}
}