using System;
using UnityEngine;

namespace n_Game.Music
{
	public enum SoundsTypes
	{ hit, footstep}

	[RequireComponent(typeof(AudioSource))]
	public class GameSound : MonoBehaviour
	{
		private AudioSource source = null;
		[Header("Game state")]
		[SerializeField]private n_MenuFSM.GameState gameState = null;

		[Header("Heroes themes")]
		[SerializeField]private AudioClip hitSound = null;
		[SerializeField]private AudioClip footstepSound = null;
		//TBD треки

		void Awake()
		{
			source = GetComponent<AudioSource>();
			gameState.OnPause += OnPause;
			gameState.OnUnpause += OnUnpause;
			GameSounds.OnGameSoundsVolumeChanged += RefreshVolume;
			RefreshVolume();
		}

		void RefreshVolume()
		{
			source.volume = GameSounds.GameSoundsVolume;
		}
		
		void OnDestroy()
		{
			GameSounds.OnGameSoundsVolumeChanged -= RefreshVolume;
			gameState.OnPause -= OnPause;
			gameState.OnUnpause -= OnUnpause;
		}

		public void PlaySound(SoundsTypes type)
		{
			switch(type)
			{
				case SoundsTypes.hit:
					source.clip = hitSound;
					break;
				case SoundsTypes.footstep:
					source.clip = footstepSound;
					break;
				default:
				throw new NotImplementedException();
			}
			source.Play();
		}

		void OnPause()
		{
			source.Pause();
		}

		void OnUnpause()
		{
			source.UnPause();
		}
	}
}
