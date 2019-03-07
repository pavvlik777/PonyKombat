using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_GameSounds
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicController : MonoBehaviour
	{
		private AudioSource source = null;
		[SerializeField]private n_MenuFSM.GameState gameState = null;

		void Awake()
		{
			source = GetComponent<AudioSource>();
			if(gameState != null)
			{
				gameState.OnPause += OnPause;
				gameState.OnUnpause += OnUnpause;
			}
			GameSounds.OnMenuMusicVolumeChanged += RefreshVolume;
			RefreshVolume();
		}

		void RefreshVolume()
		{
			source.volume = GameSounds.MenuMusicVolume;
		}
		
		void OnDestroy()
		{
			GameSounds.OnMenuMusicVolumeChanged -= RefreshVolume;
			if(gameState != null)
			{
				gameState.OnPause -= OnPause;
				gameState.OnUnpause -= OnUnpause;
			}
		}

		void OnPause()
		{
			source.Play();
		}

		void OnUnpause()
		{
			source.Stop();
		}
	}
}
