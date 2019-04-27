using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class AICombosRegistration : MonoBehaviour, IComboRegistration
	{
		private List<float> damages = new List<float>{};

		private int currentCombo = -1;
		public int CurrentCombo
		{ get { return currentCombo; } }
		public int AmountOfCombos
		{ get { return damages.Count; } }

		public event Action OnResetCombo;
		public void ResetCombo()
		{
			OnResetCombo?.Invoke();
			currentCombo = -1;
		}

		public void ChooseCombo(int number)
		{
			if(number < -1 || number >= damages.Count)
				throw new ArgumentOutOfRangeException($"{number} - incorrect combo number");
			currentCombo = number;
		}

		public float CurrentComboDamage
		{
			get
			{
				return damages[currentCombo];
			}
		}

		public void InitSet(Dictionary<string, float> input)
		{
			foreach(var cur in input)
				damages.Add(cur.Value);
		}
	}
}
