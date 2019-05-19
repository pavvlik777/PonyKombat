using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace n_Game.Combat.UI
{
	[Serializable]
	public class AchievementData
	{
		public string localizationTitle = "";
		public string localizationDescr = "";
	}

	public class AchievementUI : MonoBehaviour
	{
		private float currentValue;
		public event Action ClockEnded;

		[Header("Achievements data")]
		[SerializeField]private AchievementData[] achievementsData = null;

		[Header("Achievement object")]
		[SerializeField]private GameObject achievementMessage = null;
		[SerializeField]private n_MenuFSM.LocalizationComponent objectTitle = null;
		[SerializeField]private n_MenuFSM.LocalizationComponent objectDescr = null;

		[Header("Sound controller")]
		[SerializeField]private n_GameSounds.SoundsController m_SoundsController = null;

		void ShowMessage(int id)
		{
			objectTitle.LocalizationText = achievementsData[id].localizationTitle;
			objectDescr.LocalizationText = achievementsData[id].localizationDescr;
			achievementMessage.SetActive(true);
		}

		void HideMessage()
		{
			achievementMessage.SetActive(false);
		}

		public void StartClock(float start, int id)
		{
			currentValue = start;
			m_SoundsController.PlayAchievementSound();
			ShowMessage(id);
			StartCoroutine(Clock());
		}

		public void StopClock()
		{
			HideMessage();
			StopAllCoroutines();
		}

		IEnumerator Clock()
		{
			while(currentValue > 0f)
			{
				currentValue -= GameTime.deltaTime;
				yield return null;
			}
			HideMessage();
			ClockEnded?.Invoke();
			yield break;
		}
	}
}
