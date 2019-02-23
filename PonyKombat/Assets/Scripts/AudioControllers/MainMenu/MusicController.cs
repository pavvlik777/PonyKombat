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

		void Awake()
		{
			source = GetComponent<AudioSource>();
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
		}
	}
}
