using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Music
{
	public class AnnouncementController : MonoBehaviour
	{
		private AudioSource source = null;
		[Header("Rounds")]
		[SerializeField]private AudioClip[] roundsSounds = null;
		[Header("Heroes")]
		[SerializeField]private AudioClip applejackWins = null;
		
		[Header("Game state object")]
		[SerializeField]private n_MenuFSM.GameState gameState = null;

		void Awake()
		{
			source = GetComponent<AudioSource>();
			gameState.OnPause += OnPause;
			gameState.OnUnpause += OnUnpause;
			GameSounds.OnGameSoundsVolumeChanged += RefreshVolume;
			RefreshVolume();
		}

		void OnPause()
		{
			source.Pause();
		}

		void OnUnpause()
		{
			source.UnPause();
		}

		void RefreshVolume()
		{
			source.volume = GameSounds.GameSoundsVolume;
		}

		public void PlayRoundSound(int number)
		{
			source.clip = roundsSounds[number];
			source.Play();
		}

		public void PlayHeroWin(HeroesNames heroName)
		{
			switch(heroName)
			{
				case HeroesNames.Applejack:
					source.clip = applejackWins;
				break;
				default:
				throw new NotImplementedException();
			}
			source.Play();
		}
		
		void OnDestroy()
		{
			GameSounds.OnGameSoundsVolumeChanged -= RefreshVolume;
			gameState.OnPause -= OnPause;
			gameState.OnUnpause -= OnUnpause;
		}
	}
}
