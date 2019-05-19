using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUser
{
	public static bool isLogined = false;

	public static string login;
	public static string password;

	public static int wins;
	public static int draws;
	public static int loses;
	
	public static bool[] achievementStatus = new bool[4];

	public static void CompleteAchievement(int id)
	{
		GameConsole.AddMessage($"Achievement_{id} unlocked");
		achievementStatus[id] = true;
	}
}
