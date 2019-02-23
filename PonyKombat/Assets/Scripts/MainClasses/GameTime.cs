using System;
using UnityEngine;

public class GameTime
{
	public static float timeScale = Time.timeScale;

	private static float m_deltaTime = Time.deltaTime;
	public static float deltaTime { get { return m_deltaTime * timeScale; } }
	public static float unscaledDeltaTime
	{
		get
		{
			return m_deltaTime;
		}
	}

	private static float m_fixedDeltaTime = Time.fixedDeltaTime;
	public static float fixedDeltaTime 
	{ 
		get 
		{ return m_fixedDeltaTime * timeScale; }
		set
		{ m_fixedDeltaTime = value; }
	}
	public static float unscaledFixedDeltaTime
	{
		get
		{
			return m_fixedDeltaTime;
		}
	}
}
