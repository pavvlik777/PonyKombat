using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Music
{
	[RequireComponent(typeof(AudioSource))]
	public class GameMusicController : MonoBehaviour
	{
		private AudioSource source = null;
		[SerializeField]private bool IsInGame = true;
		[SerializeField]private n_MenuFSM.GameState gameState = null;

		void Awake()
		{
			source = GetComponent<AudioSource>();
			gameState.OnPause += OnPause;
			gameState.OnUnpause += OnUnpause;
			if(IsInGame)
			{
				GameSounds.OnGameMusicVolumeChanged += RefreshVolume;
			}
			else
			{
				GameSounds.OnMenuMusicVolumeChanged += RefreshVolume;
			}
			RefreshVolume();
		}

		void RefreshVolume()
		{
			if(IsInGame)
				source.volume = GameSounds.GameMusicVolume;
			else
				source.volume = GameSounds.MenuMusicVolume;
		}
		
		void OnDestroy()
		{
			if(IsInGame)
				GameSounds.OnGameMusicVolumeChanged -= RefreshVolume;
			else
				GameSounds.OnMenuMusicVolumeChanged -= RefreshVolume;
		}

		void OnPause()
		{
			if(IsInGame)
				source.Pause();
			else
				source.Play();
		}

		void OnUnpause()
		{
			if(IsInGame)
				source.UnPause();
			else
				source.Stop();
		}
	}
}
