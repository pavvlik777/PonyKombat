using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM.ControlsSettings
{
	public class ButtonBlock : MonoBehaviour
	{
		#region ButtonBlock
		[Serializable]
		struct ButtonComponent
		{
			public Button button;
			public Text text;

			public ButtonComponent(Button _button, Text _text)
			{
				button = _button;
				text = _text;
			}
		}
		#endregion

		public string buttonName;
		[SerializeField]private ButtonComponent mainButton = new ButtonComponent();
		[SerializeField]private ButtonComponent altButton = new ButtonComponent();
		[SerializeField]private Text DescriptionText = null;
		[SerializeField]private ControlsSettingsState stateObj = null;

		void Awake()
		{
			ControlsSettingsState.OnChangingButtonFinished += RefreshButtonValue;
			RefreshButtonValue ();
		}

		public void SetButtons()
		{
			mainButton.button.onClick.AddListener(() => {ChangingMainButtonInited();});
			mainButton.button.onClick.AddListener(() => {stateObj.InitChangeMainButton(buttonName);});
			altButton.button.onClick.AddListener(() => {ChangingAltButtonInited();});
			altButton.button.onClick.AddListener(() => {stateObj.InitChangeAltButton(buttonName);});
			RefreshButtonValue ();
		}

		public void ChangingMainButtonInited()
		{
			mainButton.text.text = "-----";
		}

		public void ChangingAltButtonInited()
		{
			altButton.text.text = "-----";
		}

		void RefreshButtonValue()
		{
			DescriptionText.text = GameInput.GetButtonDescription(buttonName);
			mainButton.text.text = FixKeycodesNames(GameInput.GetButtonFromString (buttonName, true));
			altButton.text.text = FixKeycodesNames(GameInput.GetButtonFromString (buttonName, false));
		}

		string FixKeycodesNames(string input)//TBD from XML
		{
			switch(input)
			{
				case "None":
					return "-----";
				case "Alpha0":
					return "0";
				case "Alpha1":
					return "1";
				case "Alpha2":
					return "2";
				case "Alpha3":
					return "3";
				case "Alpha4":
					return "4";
				case "Alpha5":
					return "5";
				case "Alpha6":
					return "6";
				case "Alpha7":
					return "7";
				case "Alpha8":
					return "8";
				case "Alpha9":
					return "9";
			}
			return input;
		}
	}
}
