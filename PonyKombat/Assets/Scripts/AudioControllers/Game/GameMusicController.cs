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
		[SerializeField]private n_MenuFSM.GameState gameState = null;
		//TBD треки

		void Awake()
		{
			source = GetComponent<AudioSource>();
			gameState.OnPause += OnPause;
			gameState.OnUnpause += OnUnpause;
			GameSounds.OnGameMusicVolumeChanged += RefreshVolume;
			RefreshVolume();
		}

		void RefreshVolume()
		{
			source.volume = GameSounds.GameMusicVolume;
		}
		
		void OnDestroy()
		{
			GameSounds.OnGameMusicVolumeChanged -= RefreshVolume;
			gameState.OnPause -= OnPause;
			gameState.OnUnpause -= OnUnpause;
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
