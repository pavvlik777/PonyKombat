using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.UI
{
	public class GameUI : MonoBehaviour
	{
		private Canvas m_Canvas;
		[SerializeField]private UITimer m_Timer = null;
		[SerializeField]private GameMessages m_GameMessages = null;
		[SerializeField]private GameResult m_GameResult = null;
		[SerializeField]private AchievementUI m_AchievementUI = null;

		public event Action<HeroController, bool> OnClockEnded; 

		void Awake()
		{
			m_Canvas = GetComponent<Canvas>();
		}

		public void ShowUI()
		{
			m_Canvas.enabled = true;
		}

		public void HideUI()
		{
			m_Canvas.enabled = false;
		}

		public void ShowGameResult(string s)
		{
			m_GameResult.ShowMessage(s);
		}

		public void ShowMessage(string s)
		{
			m_GameMessages.ShowMessage(s);
		}

		public void StartClock(float start)
		{
			m_Timer.StartClock(start);
			m_Timer.ClockEnded += ClockEnded;
		}

		void ClockEnded()
		{
			OnClockEnded?.Invoke(null, false);
			m_Timer.ClockEnded -= ClockEnded;
		}

		public void StopClock()
		{
			m_Timer.StopClock();
			m_Timer.ClockEnded -= ClockEnded;
		}

		public void ShowAchievement(int id)
		{
			if(!GameUser.achievementStatus[id])
			{
				GameUser.CompleteAchievement(id);
				m_AchievementUI.StartClock(5, id);
			}
		}
	}
}
