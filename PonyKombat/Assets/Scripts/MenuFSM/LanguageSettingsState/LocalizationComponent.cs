using System;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	[RequireComponent(typeof(Text))]
	public class LocalizationComponent : MonoBehaviour
	{
		private Text _text;
		[SerializeField]private string _LocalizationText = null;
		public string LocalizationText
		{ set { _LocalizationText = value;
				RefreshText();} }

		void Awake()
		{
			_text = GetComponent<Text>();
			RefreshText();
			GameLanguages.OnLanguageChanged += RefreshText;
		}

		void RefreshText()
		{
			if(_LocalizationText != "")
			{
				if(_text == null)
					_text = GetComponent<Text>();
				_text.text = GameLanguages.GetCurrentLocalization(_LocalizationText);
			}
		}

		void OnDestroy()
		{
			GameLanguages.OnLanguageChanged -= RefreshText;
		}
	}
}
