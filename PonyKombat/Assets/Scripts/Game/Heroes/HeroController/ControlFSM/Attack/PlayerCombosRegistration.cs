using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class PlayerCombosRegistration : MonoBehaviour, IComboRegistration
	{
		[Serializable]
		private struct ComboData
		{
			[SerializeField]private List<Button> comboButtons;
			private float comboDamage;
			public float ComboDamage { get { return comboDamage; } }

			public bool IsConditionsMet(List<Button> pressedButtons)
			{
				if(comboButtons.Count > pressedButtons.Count)
					return false;
				for(int i = 0; i < comboButtons.Count; i++)
				{
					if(comboButtons[comboButtons.Count - 1 - i] != pressedButtons[pressedButtons.Count - 1 - i])
						return false;
				}
				return true;
			}

			public int ComboLength { get { return comboButtons.Count; } }

			public ComboData(List<Button> init, float damage)
			{
				comboButtons = init;
				comboDamage = damage;
			}

			public ComboData(string init, float damage)
			{
				comboDamage = damage;
				comboButtons = new List<Button>{};
				init = init.ToUpper();
				foreach(var cur in init)
				{
					switch(cur)
					{
						case 'X':
							comboButtons.Add(Button.X);
							break;
						case 'Y':
							comboButtons.Add(Button.Y);
							break;
						case 'A':
							comboButtons.Add(Button.A);
							break;
						case 'B':
							comboButtons.Add(Button.B);
							break;
						case 'L':
							comboButtons.Add(Button.Left);
							break;
						case 'R':
							comboButtons.Add(Button.Right);
							break;
						case 'U':
							comboButtons.Add(Button.Up);
							break;
						case 'D':
							comboButtons.Add(Button.Down);
							break;
						default:
							throw new ArgumentOutOfRangeException("Incorrect combo symbol");
					}
				}
			}
		}

		private enum Button
		{ Up, Down, Right, Left, X, Y, A, B }

		private List<Button> pressedButtons = new List<Button>{};
		[Header("Game state")]
		[SerializeField]private n_MenuFSM.GameState m_GameState = null;

		[Header("Combos data")]
		private List<ComboData> awailableCombos = new List<ComboData>{};
		[SerializeField]private float combosClearDelay = 0.3f;
		[SerializeField]private int maxPressedButtonsAmount = 10;

		private int currentCombo = -1;
		public int CurrentCombo { get { return currentCombo; } }
		private float currentComboDamage;
		public float CurrentComboDamage 
		{ 
			get 
			{ 
				return currentComboDamage;
			}
		}

		public void InitSet(Dictionary<string, float> input)
		{
			awailableCombos = new List<ComboData>{};
			foreach(var cur in input)
				awailableCombos.Add(new ComboData(cur.Key, cur.Value));
		}

		private bool IsPause = false;

		void Awake()
		{
			//GameConsole.OnUserCommand += UserCommandsReaction;
			m_GameState.OnPause += OnPause;
			m_GameState.OnUnpause += OnUnpause;
		}
		void OnDestroy()
		{
			//GameConsole.OnUserCommand -= UserCommandsReaction;
			m_GameState.OnPause -= OnPause;
			m_GameState.OnUnpause -= OnUnpause;
		}
		void OnPause()
		{
			IsPause = true;
		}
		void OnUnpause()
		{
			IsPause = false;
		}

		void OnGUI()
		{
			if(!IsPause)
			{
				Event keyEvent = Event.current;
				if (keyEvent.isKey) {
					if(keyEvent.keyCode != KeyCode.None && keyEvent.type == EventType.KeyDown)
					{
						if(GameInput.GetButtonDown("Up"))
							AddPressedButton(Button.Up);
						if(GameInput.GetButtonDown("Down"))
							AddPressedButton(Button.Down);
						else if(GameInput.GetButtonDown("Right"))
							AddPressedButton(Button.Right);
						else if(GameInput.GetButtonDown("Left"))
							AddPressedButton(Button.Left);
						else if(GameInput.GetButtonDown("X"))
							AddPressedButton(Button.X);
						else if(GameInput.GetButtonDown("Y"))
							AddPressedButton(Button.Y);
						else if(GameInput.GetButtonDown("A"))
							AddPressedButton(Button.A);
						else if(GameInput.GetButtonDown("B"))
							AddPressedButton(Button.B);
					}
				}
			}
		}

		void AddPressedButton(Button newButton)
		{
			if(pressedButtons.Count < maxPressedButtonsAmount)
			{
				pressedButtons.Add(newButton);
			}
			else
			{
				for(int i = 0; i < pressedButtons.Count - 1; i++)
					pressedButtons[i] = pressedButtons[i + 1];
				pressedButtons[pressedButtons.Count - 1] = newButton;
			}
			StopAllCoroutines();
			StartCoroutine(ComboClearDelay());
			CheckCombosConditions();
		}

		IEnumerator ComboClearDelay()
		{
			float timePassed = 0f;
			while(timePassed <= combosClearDelay)
			{
				timePassed += GameTime.deltaTime;
				yield return null;
			}
			pressedButtons.Clear();
			yield break;
		}

		void CheckCombosConditions()
		{
			currentCombo = -1;
			int currentLength = -1;
			currentComboDamage = 0f;
			for(int i = 0; i < awailableCombos.Count; i++)
			{
				if(awailableCombos[i].IsConditionsMet(pressedButtons))
				{
					if(currentLength < awailableCombos[i].ComboLength)
					{
						currentCombo = i;
						currentLength = awailableCombos[i].ComboLength;
						currentComboDamage = awailableCombos[currentCombo].ComboDamage;
					}
				}
			}
		}

		public void ResetCombo()
		{
			currentCombo = -1;
		}
	}
}
