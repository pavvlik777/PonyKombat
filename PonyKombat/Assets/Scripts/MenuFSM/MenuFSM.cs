using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_MenuFSM
{
	public static class StatesNamesExtensions
	{
		public static string GetString(this StatesNames name)
		{
			return name.ToString ().ToLower();
		}

		public static StatesNames GetStateNameFromString(this string Name)
		{
			string name = Name.ToLower ();
			switch (name) {
			case "game":
				return StatesNames.Game;
			case "mainmenu":
				return StatesNames.MainMenu;
			case "newgame":
				return StatesNames.NewGame;
			case "loadgame":
				return StatesNames.LoadGame;
			case "settings":
				return StatesNames.Settings;
			case "exitgame":
				return StatesNames.ExitGame;
			case "audiosettings":
				return StatesNames.AudioSettings;
			case "videosettings":
				return StatesNames.VideoSettings;
			case "controlssettings":
				return StatesNames.ControlsSettings;
			case "languagesettings":
				return StatesNames.LanguageSettings;
			case "userlogin":
				return StatesNames.UserLogin;
			case "userstats":
				return StatesNames.UserStats;
			default:
				throw new NotImplementedException ("Such name didn't implemented yet");
			}
		}
	}

	[Serializable]
	public enum StatesNames
	{ Game, MainMenu, NewGame, LoadGame, Settings, ExitGame, AudioSettings, VideoSettings, ControlsSettings, Console, LanguageSettings, UserLogin, UserStats }
 
	public class MenuFSM : MonoBehaviour
	{
		private State currentState;
		private State previousState;
		private bool IsChangeStateAllowed;

		[SerializeField]private StatesNames initialState = StatesNames.MainMenu;
		[SerializeField]private State[] states = null;
		[SerializeField]private bool isEscPressAllowed = false;

		[ContextMenu("ShowAllStates")]
		public void ShowAllStates()
		{
			foreach (State cur in states) 
				cur.InitialObjectSwitch (true);
		}

		[ContextMenu("HideAllStates")]
		public void HideAllStates()
		{
			foreach (State cur in states) 
				cur.InitialObjectSwitch (false);
		}

		public void ChangeState(string _name)
		{
			StatesNames temp = _name.GetStateNameFromString ();
			ChangeState (temp);
		}
		public void ChangeState(StatesNames _name)
		{
			if(_name == currentState.stateName || !IsChangeStateAllowed)
				return;
			if(!currentState.CheckNewState(_name))
				throw new ArgumentOutOfRangeException($"Transition {currentState.stateName} - {_name} not allowed");
			currentState.LeaveState (_name);
			previousState = currentState;
			State newState = GetStateFromName(_name);
			currentState = newState;
			currentState.EnterState();
		}

		State GetStateFromName(StatesNames _name)
		{
			foreach (State cur in states) {
				if (cur.stateName == _name)
					return cur;
			}
			throw new ArgumentOutOfRangeException ();
		}

		public void LockChangeState()
		{
			IsChangeStateAllowed = false;
		}

		public void UnlockChangeState()
		{
			IsChangeStateAllowed = true;
		}

		void Start()
		{
			IsChangeStateAllowed = true;
			currentState = GetStateFromName (initialState);
			previousState = null;
			foreach (State cur in states) {
				if (cur.stateName == initialState)
					cur.EnterState ();
				else
					cur.InitialObjectSwitch (false);
			}
		}

		void Update()
		{
			currentState.UpdateState();
			if (Input.GetKeyDown (KeyCode.Escape) && isEscPressAllowed)//Game input reserved buttons
				if(currentState.stateName != StatesNames.Console)
					ChangeState (currentState.StateIfEscPressed);
				else
					ChangeState (previousState.stateName);
			else if(Input.GetKeyDown(KeyCode.BackQuote))
			{
				if(currentState.stateName != StatesNames.Console)
					ChangeState (StatesNames.Console);
				else
					ChangeState (previousState.stateName);
			}
		}
	}

	public abstract class State : MonoBehaviour
	{
		public StatesNames stateName;

		[SerializeField]protected GameObject stateObject;
		[SerializeField]protected StatesNames[] allowedTransitions;
		[SerializeField]protected StatesNames stateIfEscPressed;
						public StatesNames StateIfEscPressed { get { return stateIfEscPressed; } }
						
		public bool CheckNewState(StatesNames newState)
		{
			for (int i = 0; i < allowedTransitions.Length; i++) {
				if (newState == allowedTransitions [i])
					return true;
			}
			return false;
		}

		protected void SwitchStateObject(bool state)
		{
			stateObject.SetActive (state);
		}
		public void InitialObjectSwitch(bool isInit)
		{
			stateObject.SetActive (isInit);
		}

		public virtual void LeaveState(StatesNames newState)
		{ 
			SwitchStateObject (false);
		}
		public virtual void EnterState()
		{ 
			SwitchStateObject (true);
		}
		public virtual void UpdateState()
		{ }
	}
}
