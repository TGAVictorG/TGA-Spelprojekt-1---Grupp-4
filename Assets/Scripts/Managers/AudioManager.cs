using UnityEngine;

namespace Managers
{
	//-------------------------------------------------------------------------
    public class AudioManager : MonoBehaviour
    {
	    #region Private Serializable Fields
        
		[Header("Follow Settings")]
		[SerializeField] private bool _myIsFollowEnabled = false;
		[SerializeField] private Transform _myPlayer = null;
		
		[Header("Audio Sources | UI")]
		[SerializeField] private AudioSource[] _myUISources = null;
		
        [Header("Audio Sources | Music")]
        [SerializeField] private AudioSource[] _myMusicSources = null;
        
        [Header("Audio Sources | SFX")]
        [SerializeField] private AudioSource[] _mySFXSources = null;
		
		[Header("Audio Sources | Gameplay")]
		[SerializeField] private AudioSource[] _myGameplaySources = null;
		
		[Header("Audio Sources | Voice")]
		[SerializeField] private AudioSource[] _myVoiceSources = null;
		
		#endregion
		
		
        #region Static Entities

        private static AudioManager _ourInstance = null;
        public static AudioManager Instance => _ourInstance;
		
		#endregion
        
		
        #region Unity Methods
        
        //-------------------------------------------------
        private void Awake()
        {
	        _ourInstance = this;
        }
        
        //-------------------------------------------------
        private void Start()
        {
#if UNITY_EDITOR
	        if (_myIsFollowEnabled && !_myPlayer)
	        {
		        Debug.LogError("Player Transform is NULL.");
	        }
#endif
        }

        //-------------------------------------------------
        private void Update()
        {
	        // if game is paused => return

	        if (_myIsFollowEnabled)
	        {
		        FollowPlayer();
	        }
        }

        //-------------------------------------------------
        private void OnEnable()
        {
	        // Subscribe to event when player dies to stop the music or all sounds, depending on need
	        // e.g.
	        // PlaneController.OnDeath += StopMusic;
	        // or
	        // PlaneController.OnDeath += StopAllSounds;
        }
        
        //-------------------------------------------------
        private void OnDisable()
        {
	        // e.g.
	        // PlaneController.OnDeath -= StopMusic;
	        // or
	        // PlaneController.OnDeath -= StopAllSounds;
        }

        #endregion


		#region Private Methods
		
		//-------------------------------------------------
		private void FollowPlayer()
		{
			if (_myPlayer)
			{
				transform.position = _myPlayer.position;
			}
		}

		//-------------------------------------------------
		private void PlayClip(int aSourceIndex, AudioSource[] someSources)
		{
			// NOTE: Safeguard against out of range and null exceptions
			if (aSourceIndex > someSources.Length - 1 || !someSources[aSourceIndex]) return;
			
			// NOTE: Only stop if already playing
			if (someSources[aSourceIndex].isPlaying)
			{
				someSources[aSourceIndex].Stop();
			}

			someSources[aSourceIndex].Play();
		}
		
		//-------------------------------------------------
		private void StopClip(int aSourceIndex, AudioSource[] someSources)
		{
			if (someSources.Length > 0 && someSources[aSourceIndex])
			{
				someSources[aSourceIndex].Stop();
			}
		}

		#endregion


		#region Public Methods
		
		#region Play Clips
		
		//-------------------------------------------------
		public void PlayMusicClip(int aSourceIndex)
		{
			PlayClip(aSourceIndex, _myMusicSources);
		}
		
		//-------------------------------------------------
		public void PlayUIClip(int aSourceIndex)
		{
			PlayClip(aSourceIndex, _myUISources);
		}
		
		//-------------------------------------------------
		public void PlaySFXClip(int aSourceIndex)
		{
			PlayClip(aSourceIndex, _mySFXSources);
		}
		
		//-------------------------------------------------
		public void PlayGameplayClip(int aSourceIndex)
		{
			PlayClip(aSourceIndex, _myGameplaySources);
		}
		
		//-------------------------------------------------
		public void PlayVoiceClip(int aSourceIndex)
		{
			PlayClip(aSourceIndex, _myVoiceSources);
		}
		
		#endregion

		#region Stop Clips
		
		//-------------------------------------------------
		public void StopMusicClip(int aSourceIndex)
		{
			StopClip(aSourceIndex, _myMusicSources);
		}
		
		//-------------------------------------------------
		public void StopUIClip(int aSourceIndex)
		{
			StopClip(aSourceIndex, _myUISources);
		}
		
		//-------------------------------------------------
		public void StopSFXClip(int aSourceIndex)
		{
			StopClip(aSourceIndex, _mySFXSources);
		}
		
		//-------------------------------------------------
		public void StopGameplayClip(int aSourceIndex)
		{
			StopClip(aSourceIndex, _myGameplaySources);
		}
		
		//-------------------------------------------------
		public void StopVoiceClip(int aSourceIndex)
		{
			StopClip(aSourceIndex, _myVoiceSources);
		}
		
		#endregion

		#region General

		//-------------------------------------------------
		/// <summary>
		/// Stops all sounds that are currently playing
		/// </summary>
		public void StopAllSounds()
		{
			if (_myMusicSources.Length > 0)
			{
				foreach (AudioSource source in _myMusicSources)
				{
					if (source && source.isPlaying)
					{
						source.Stop();
					}
				}
			}
			
			if (_myUISources.Length > 0)
			{
				foreach (AudioSource source in _myUISources)
				{
					if (source && source.isPlaying)
					{
						source.Stop();
					}
				}
			}
			
			if (_mySFXSources.Length > 0)
			{
				foreach (AudioSource source in _mySFXSources)
				{
					if (source && source.isPlaying)
					{
						source.Stop();
					}
				}
			}
			
			if (_myGameplaySources.Length > 0)
			{
				foreach (AudioSource source in _myGameplaySources)
				{
					if (source && source.isPlaying)
					{
						source.Stop();
					}
				}
			}
			
			if (_myVoiceSources.Length > 0)
			{
				foreach (AudioSource source in _myVoiceSources)
				{
					if (source && source.isPlaying)
					{
						source.Stop();
					}
				}
			}
		}
		
		//-------------------------------------------------
		public void StopMusic()
		{
			for (int sourceIndex = 0; sourceIndex < _myMusicSources.Length; ++sourceIndex)
			{
				if (_myMusicSources.Length > 1 && _myMusicSources[sourceIndex])
				{
					StopClip(sourceIndex, _myMusicSources);
				}
			}
		}

		#endregion
		
		#endregion
    }
}