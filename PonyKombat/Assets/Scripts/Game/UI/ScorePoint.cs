using System;
using UnityEngine;
using UnityEngine.UI;

namespace n_Game.Combat.UI
{
	public class ScorePoint : MonoBehaviour
	{
		[SerializeField]private Image value = null;

		public void SetValue(Color color)
		{
			value.color = color;
		}
	}
}
