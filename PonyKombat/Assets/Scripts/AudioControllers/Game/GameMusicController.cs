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
		[Header("Game state")]
		[SerializeField]private n_MenuFSM.GameState gameState = null;

		[Header("Heroes themes")]
		[SerializeField]private AudioClip ApplejackMusic = null;
		[SerializeField]private AudioClip FluttershyMusic = null;
		[SerializeField]private AudioClip PinkiePieMusic = null;
		[SerializeField]private AudioClip StarlightGlimmerMusic = null;
		[SerializeField]private AudioClip RainbowDashMusic = null;
		[SerializeField]private AudioClip RarityMusic = null;
		[SerializeField]private AudioClip TwilightSparkleMusic = null;
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

		public void PlayMusic(HeroesNames hero)
		{
			switch(hero)
			{
				case HeroesNames.Applejack:
					source.clip = ApplejackMusic;
					break;
				case HeroesNames.Fluttershy:
					source.clip = FluttershyMusic;
					break;
				case HeroesNames.PinkiePie:
					source.clip = PinkiePieMusic;
					break;
				case HeroesNames.StarlightGlimmer:
					source.clip = StarlightGlimmerMusic;
					break;
				case HeroesNames.RainbowDash:
					source.clip = RainbowDashMusic;
					break;
				case HeroesNames.Rarity:
					source.clip = RarityMusic;
					break;
				case HeroesNames.TwilightSparkle:
					source.clip = TwilightSparkleMusic;
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
