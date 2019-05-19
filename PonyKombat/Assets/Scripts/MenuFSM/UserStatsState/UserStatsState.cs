using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	public class UserStatsState : State
	{
		[SerializeField]private Sprite achievementLocked = null;
		[SerializeField]private Sprite achievementUnlocked = null;

		[SerializeField]private AchievementListComponent[] achievements = null;

		[SerializeField]private Text matchesStats = null;

		public override void EnterState()
		{
			for(int i = 0; i < achievements.Length; i++)
				achievements[i].SetAchievementStatus(GameUser.achievementStatus[i] ? achievementUnlocked : achievementLocked);
			matchesStats.text = $"{GameUser.wins}/{GameUser.draws}/{GameUser.loses}";
			SwitchStateObject (true);
		}
	}
}
