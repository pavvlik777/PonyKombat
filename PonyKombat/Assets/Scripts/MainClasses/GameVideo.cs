using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVideo
{
	public static Resolution ScreenResolution;

	public static void SetDefaultSettings() //TBD
	{
		Screen.SetResolution(1600, 900, true);
		GameVideo.ScreenResolution = Screen.currentResolution;
	}
}