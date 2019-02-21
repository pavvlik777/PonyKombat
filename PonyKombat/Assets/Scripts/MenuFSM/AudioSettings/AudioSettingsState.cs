using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	public class AudioSettingsState : State
	{
		[Header("Volume sliders")]
		[SerializeField]private Slider MenuMusicSlider = null;
		[SerializeField]private Slider MenuSoundsSlider = null;
		[SerializeField]private Slider GameMusicSlider = null;
		[SerializeField]private Slider GameSoundsSlider = null;

		void Awake()
		{
			RefreshAllVolumes();

			MenuMusicSlider.onValueChanged.AddListener((value)=>{ChangeMenuMusicVolume(value);});
			MenuSoundsSlider.onValueChanged.AddListener((value)=>{ChangeMenuSoundsVolume(value);});
			GameMusicSlider.onValueChanged.AddListener((value)=>{ChangeGameMusicVolume(value);});
			GameSoundsSlider.onValueChanged.AddListener((value)=>{ChangeGameSoundsVolume(value);});
		}

		public void ChangeMenuMusicVolume(float newVolume)
		{
			GameSounds.MenuMusicVolume = newVolume;
		}
		public void ChangeMenuSoundsVolume(float newVolume)
		{
			GameSounds.MenuSoundsVolume = newVolume;
		}
		public void ChangeGameMusicVolume(float newVolume)
		{
			GameSounds.GameMusicVolume = newVolume;
		}
		public void ChangeGameSoundsVolume(float newVolume)
		{
			GameSounds.GameSoundsVolume = newVolume;
		}

		void RefreshAllVolumes()
		{
			MenuMusicSlider.value = GameSounds.MenuMusicVolume;
			MenuSoundsSlider.value = GameSounds.MenuSoundsVolume;
			GameMusicSlider.value = GameSounds.GameMusicVolume;
			GameSoundsSlider.value = GameSounds.GameSoundsVolume;
		}
	}
}