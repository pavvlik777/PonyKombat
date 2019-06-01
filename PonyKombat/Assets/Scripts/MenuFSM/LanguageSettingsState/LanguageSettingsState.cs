using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_MenuFSM
{
	public class LanguageSettingsState : State
	{
		[SerializeField]private UnityEngine.UI.Toggle EnglishToggle = null;
		[SerializeField]private UnityEngine.UI.Toggle RussianToggle = null;
		[SerializeField]private UnityEngine.UI.Toggle BelarusianToggle = null;

		public void SetLanguage(string language)
		{
			GameLanguages.ChangeCurrentLanguage(language);
		}

		public override void EnterState()
		{
			SwitchStateObject (true);
			string language = GameLanguages.GetCurrentLanguage();
			switch(language)
			{
				case "English":
					EnglishToggle.isOn = true;
					break;
				case "Russian":
					RussianToggle.isOn = true;
					break;
				case "Belarusian":
					BelarusianToggle.isOn = true;
					break;
				default:
					throw new NotImplementedException($"{language} not implemented yet");
			}
		}
	}
}
