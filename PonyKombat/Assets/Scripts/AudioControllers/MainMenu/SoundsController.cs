using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_GameSounds
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundsController : MonoBehaviour
	{
		private enum Sounds
		{ hit, click, back, achievement }

		private AudioSource source = null;
		[Header("Sounds")]
		[SerializeField]private AudioClip menuHit = null;
		[SerializeField]private AudioClip menuClick = null;
		[SerializeField]private AudioClip menuBack = null;
		[SerializeField]private AudioClip achievementSound = null;
		
		[Header("Game state object")]
		[SerializeField]private n_MenuFSM.GameState gameState = null;

		void Awake()
		{
			source = GetComponent<AudioSource>();
			if(gameState != null)
				gameState.OnUnpause += OnUnpause;
			GameSounds.OnMenuSoundsVolumeChanged += RefreshVolume;
			RefreshVolume();
		}

		void OnUnpause()
		{
			source.Stop();
		}

		void RefreshVolume()
		{
			source.volume = GameSounds.MenuSoundsVolume;
		}
		
		void OnDestroy()
		{
			GameSounds.OnMenuSoundsVolumeChanged -= RefreshVolume;
			if(gameState != null)
				gameState.OnUnpause -= OnUnpause;
		}

		public void PlayMenuHit()
		{
			PlaySound(Sounds.hit);
		}

		public void PlayMenuClick()
		{
			PlaySound(Sounds.click);
		}

		public void PlayMenuBack()
		{
			PlaySound(Sounds.back);
		}

		public void PlayAchievementSound()
		{
			PlaySound(Sounds.achievement);
		}

		void PlaySound(Sounds i)
		{
			AudioClip playClip = null;
			switch(i)
			{
				case Sounds.hit:
					playClip = menuHit;
				break;
				case Sounds.click:
					playClip = menuClick;
				break;
				case Sounds.back:
					playClip = menuBack;
				break;
				case Sounds.achievement:
					playClip = achievementSound;
				break;
				default:
					throw new NotImplementedException("There isn't such sound");
			}
			source.clip = playClip;
			source.Play();
		}
	}
}
