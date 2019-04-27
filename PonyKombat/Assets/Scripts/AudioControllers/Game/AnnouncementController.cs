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
		[SerializeField]private AudioClip fluttershyWins = null;
		[SerializeField]private AudioClip pinkiePieWins = null;
		[SerializeField]private AudioClip starlightGlimmerWins = null;
		[SerializeField]private AudioClip rainbowDashWins = null;
		[SerializeField]private AudioClip rarityWins = null;
		[SerializeField]private AudioClip twilightSparkleWins = null;
		
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
				case HeroesNames.Fluttershy:
					source.clip = fluttershyWins;
				break;
				case HeroesNames.PinkiePie:
					source.clip = pinkiePieWins;
					break;
				case HeroesNames.StarlightGlimmer:
					source.clip = starlightGlimmerWins;
					break;
				case HeroesNames.RainbowDash:
					source.clip = rainbowDashWins;
					break;
				case HeroesNames.Rarity:
					source.clip = rarityWins;
					break;
				case HeroesNames.TwilightSparkle:
					source.clip = twilightSparkleWins;
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
