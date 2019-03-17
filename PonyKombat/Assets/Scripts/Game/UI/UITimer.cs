using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace n_Game.Combat.UI
{
	public class UITimer : MonoBehaviour
	{
		private float currentValue;
		public event Action ClockEnded;

		[SerializeField]private Text text = null;

		public void StartClock(float start)
		{
			currentValue = start + 1f; //to correct output
			StartCoroutine(Clock());
		}

		public void StopClock()
		{
			StopAllCoroutines();
		}

		IEnumerator Clock()
		{
			while(currentValue > 0f)
			{
				currentValue -= GameTime.deltaTime;
				if(currentValue < 0f)
					break;
				else
					text.text = $"{Math.Floor(currentValue)}";
				yield return null;
			}
			text.text = "0";
			ClockEnded?.Invoke();
			yield break;
		}
	}
}
