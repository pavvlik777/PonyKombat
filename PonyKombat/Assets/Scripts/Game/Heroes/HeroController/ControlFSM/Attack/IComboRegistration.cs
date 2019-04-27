using System;
using System.Collections.Generic;

namespace n_Game.Combat.Control
{
	public interface IComboRegistration
	{
		int CurrentCombo { get; }
		float CurrentComboDamage { get; }

		void InitSet(Dictionary<string, float> input);
		event Action OnResetCombo;
		void ResetCombo();
	}
}
