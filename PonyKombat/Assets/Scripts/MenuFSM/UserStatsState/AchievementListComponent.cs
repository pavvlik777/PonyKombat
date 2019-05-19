using System;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	public class AchievementListComponent : MonoBehaviour
	{
		[SerializeField]private Image icon = null;

		public void SetAchievementStatus(Sprite status)
		{
			icon.sprite = status;
		}
	}
}
