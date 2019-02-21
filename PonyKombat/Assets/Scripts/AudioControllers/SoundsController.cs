using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_GameSounds
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundsController : MonoBehaviour
	{
		private AudioSource source = null;

		void Awake()
		{
			source = GetComponent<AudioSource>();
			GameSounds.OnMenuSoundsVolumeChanged += RefreshVolume;
			RefreshVolume();
		}

		void RefreshVolume()
		{
			source.volume = GameSounds.MenuSoundsVolume;
		}
		
		void OnDestroy()
		{
			GameSounds.OnMenuSoundsVolumeChanged -= RefreshVolume;
		}
	}
}
