using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_MenuFSM.ControlsSettings;

namespace n_MenuFSM
{
	public class ControlsSettingsState : State
	{
		private KeyCode[] reservedButtons = {
			KeyCode.Escape, KeyCode.BackQuote, KeyCode.Return
		};

		public static event Action OnChangingButtonFinished;
		private bool isChangingMain = false;
		private bool isChangingAlt = false;
		private string changingButton = "";

		[SerializeField]private GameObject buttonBlockExample = null;
		[SerializeField]private RectTransform contentGO = null;

		public override void LeaveState(StatesNames newState)
		{
			isChangingMain = false;
			isChangingAlt = false;
			changingButton = "";

			OnChangingButtonFinished?.Invoke ();
			SwitchStateObject (false);

		}
		public override void EnterState()
		{
			isChangingMain = false;
			isChangingAlt = false;
			changingButton = "";

			SwitchStateObject (true);
		}
		void OnGUI()
		{
			Event keyEvent = Event.current;
			if (keyEvent.isKey) {
				if (isChangingMain)
					ChangeMainButton (changingButton, keyEvent.keyCode);
				else if (isChangingAlt)
					ChangeAltButton (changingButton, keyEvent.keyCode);
			}
			else if(Input.GetKey(KeyCode.LeftShift))
			{
				if (isChangingMain)
					ChangeMainButton (changingButton, KeyCode.LeftShift);
				else if (isChangingAlt)
					ChangeAltButton (changingButton, KeyCode.LeftShift);
			}
			else if(Input.GetKey(KeyCode.RightShift))
			{
				if (isChangingMain)
					ChangeMainButton (changingButton, KeyCode.RightShift);
				else if (isChangingAlt)
					ChangeAltButton (changingButton, KeyCode.RightShift);
			}
		}

		void Awake()
		{
			buttonBlockExample.SetActive (true);
			float x = buttonBlockExample.GetComponent<RectTransform> ().localPosition.x;
			float y = buttonBlockExample.GetComponent<RectTransform> ().localPosition.y;
			int i = 0;
			foreach (var cur in GameInput.GetButtonsNames()) {
				GameObject newObj = Instantiate(buttonBlockExample, contentGO);
				newObj.name = cur.Item1;
				newObj.GetComponent<ButtonBlock> ().buttonName = cur.Item1;
				newObj.GetComponent<ButtonBlock> ().SetButtons ();
				newObj.GetComponent<RectTransform> ().localPosition = new Vector3 (x, y - i++ * 80);
			}
			contentGO.sizeDelta = new Vector2 (contentGO.sizeDelta.x, 100 + (i-1) * 80); //2 * 20 + i * 60 + (i-1)*20
			buttonBlockExample.SetActive (false);
		}

		public void InitChangeMainButton(string buttonName)
		{
			if (isChangingMain || isChangingAlt)
				return;
			isChangingMain = true;
			changingButton = buttonName;
		}

		bool IsButtonReserved(KeyCode button)
		{
			foreach(var cur in reservedButtons)
				if(cur == button)
					return true;
			return false;
		}

		public void ChangeMainButton(string buttonName, KeyCode newKey)
		{
			if (IsButtonReserved(newKey))
				return;
			GameInput.ChangeButtonMainKey (buttonName, newKey);
			isChangingMain = false;
			changingButton = buttonName;

			OnChangingButtonFinished?.Invoke ();
		}

		public void InitChangeAltButton(string buttonName)
		{
			if (isChangingMain || isChangingAlt)
				return;
			isChangingAlt = true;
			changingButton = buttonName;
		}

		public void ChangeAltButton(string buttonName, KeyCode newKey)
		{
			if (IsButtonReserved(newKey))
				return;
			GameInput.ChangeButtonAltKey (buttonName, newKey);
			isChangingAlt = false;
			changingButton = buttonName;

			OnChangingButtonFinished?.Invoke ();
		}
	}
}
