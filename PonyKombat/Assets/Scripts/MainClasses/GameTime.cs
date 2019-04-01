using System;
using UnityEngine;

public class GameTime
{
	public static float timeScale = Time.timeScale;

	public static float deltaTime { get { return Time.deltaTime * timeScale; } }
	public static float unscaledDeltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	public static float fixedDeltaTime 
	{ 
		get 
		{ return Time.fixedDeltaTime * timeScale; }
		set
		{ Time.fixedDeltaTime = value; }
	}
	public static float unscaledFixedDeltaTime
	{
		get
		{
			return Time.fixedDeltaTime;
		}
	}
}
