using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSounds
{
	public static void SetDefaultVolume()
	{
		MenuSoundsVolume = 0.1f;
		GameSoundsVolume = 0.1f;
		MenuMusicVolume = 0.1f;
		GameMusicVolume = 0.1f;
	}

	private static float menuSoundsVolume = 0.1f;
	public static event Action OnMenuSoundsVolumeChanged;
	public static float MenuSoundsVolume
	{
		get  { return menuSoundsVolume; }
		set {
			if(value >= 0f && value <= 1f)
			{
				menuSoundsVolume = value;
				OnMenuSoundsVolumeChanged?.Invoke();
			}
			else
				throw new ArgumentOutOfRangeException("Incorrect volume");
		}
	}

	private static float gameSoundsVolume = 0.1f;
	public static event Action OnGameSoundsVolumeChanged;
	public static float GameSoundsVolume
	{
		get  { return gameSoundsVolume; }
		set {
			if(value >= 0f && value <= 1f)
			{
				gameSoundsVolume = value;
				OnGameSoundsVolumeChanged?.Invoke();
			}
			else
				throw new ArgumentOutOfRangeException("Incorrect volume");
		}
	}

	private static float menuMusicVolume = 0.1f;
	public static event Action OnMenuMusicVolumeChanged;
	public static float MenuMusicVolume
	{
		get  { return menuMusicVolume; }
		set {
			if(value >= 0f && value <= 1f)
			{
				menuMusicVolume = value;
				OnMenuMusicVolumeChanged?.Invoke();
			}
			else
				throw new ArgumentOutOfRangeException("Incorrect volume");
		}
	}

	private static float gameMusicVolume = 0.1f;
	public static event Action OnGameMusicVolumeChanged;
	public static float GameMusicVolume
	{
		get  { return gameMusicVolume; }
		set {
			if(value >= 0f && value <= 1f)
			{
				gameMusicVolume = value;
				OnGameMusicVolumeChanged?.Invoke();
			}
			else
				throw new ArgumentOutOfRangeException("Incorrect volume");
		}
	}
}
