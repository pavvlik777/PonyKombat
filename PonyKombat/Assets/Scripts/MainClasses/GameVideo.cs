using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVideo
{
	public static Resolution screenResolution;

	public static void SetDefaultSettings() //TBD
	{
		Screen.SetResolution(1600, 900, true);
		SetAnisotropicFiltering(false);
		SetAntiAliasing(0);
		GameVideo.screenResolution = Screen.currentResolution;
	}

	public static void SetResolution(int width, int height)
	{
		Screen.SetResolution(width, height, true);
		GameVideo.screenResolution = Screen.currentResolution;
	}

	public static bool anisotropicFiltering
	{ get { return QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable; } }

	public static int antiAliasing
	{ get { return QualitySettings.antiAliasing; } }

	public static void SetAnisotropicFiltering(bool state)
	{
		QualitySettings.anisotropicFiltering = state ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;
	}

	public static void SetAntiAliasing(int mode)
	{
		QualitySettings.antiAliasing = mode;
	}
}