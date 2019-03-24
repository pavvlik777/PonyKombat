using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_EasterEgg
{
	public class DerpyEasterEgg : MonoBehaviour
	{
		[SerializeField]private n_MenuFSM.GameState m_GameState = null;
		[SerializeField]private GameObject Derpy = null;
		private List<KeyCode> easterKeys = new List<KeyCode>{}; //muffin
		private List<KeyCode> easterCondition = new List<KeyCode>{KeyCode.M, KeyCode.U, KeyCode.F, KeyCode.F, KeyCode.I, KeyCode.N};
		
		private bool IsEasterEggActive;
		private bool IsPause;

		void Awake()
		{
			GameConsole.OnUserCommand += UserCommandsReaction;
			m_GameState.OnPause += OnPause;
			m_GameState.OnUnpause += OnUnpause;
			IsEasterEggActive = false;
		}

		void OnDestroy()
		{
			GameConsole.OnUserCommand -= UserCommandsReaction;
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

		void UserCommandsReaction(string command)
		{
			switch(command)
			{
				case "g_x=muffin":
					if(!IsEasterEggActive)
						GameConsole.AddMessage("What the hay?", false, false);
					else
						GameConsole.AddMessage("I zap u!", false, false);
					IsEasterEggActive = !IsEasterEggActive;
				break;
			}
		}
		
		void AddKey(KeyCode key)
		{
			if(easterKeys.Count < 6)
			{
				easterKeys.Add(key);
			}
			else
			{
				for(int i = 0; i < easterKeys.Count - 1; i++)
					easterKeys[i] = easterKeys[i + 1];
				easterKeys[easterKeys.Count - 1] = key;
			}
			CheckEasterEggConditions();
		}

		void OnGUI()
		{
			if(IsEasterEggActive && !IsPause)
			{
				Event keyEvent = Event.current;
				if (keyEvent.isKey) {
					if(keyEvent.keyCode != KeyCode.None && keyEvent.type == EventType.KeyDown)
						AddKey(keyEvent.keyCode);
				}
			}
		}

		void CheckEasterEggConditions()
		{
			if(easterKeys.Count < 6)
				return;
			for(int i = 0; i < 6; i++)
			{
				if(easterKeys[i] != easterCondition[i])
					return;
			}
			ActivateEasterEgg();
		}

		void ActivateEasterEgg()
		{
			GameConsole.AddMessage("PonyKombat® by Astapenko P.I. BSUIR 2019", false, false);
			StopAllCoroutines();
			Derpy.SetActive(true);
			StartCoroutine(HideEasterEgg());
		}

		IEnumerator HideEasterEgg()
		{
			float timePassed = 0f;
			while(timePassed <= 10f)
			{
				timePassed += GameTime.deltaTime;
				yield return null;
			}
			Derpy.SetActive(false);
			IsEasterEggActive = false;
			yield break;
		}
	}
}
