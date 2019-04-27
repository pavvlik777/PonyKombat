using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	public class GameState : State
	{
		[Header("Menu background")]
		[SerializeField]private Image Background = null;
		private float timeScale;

		public event Action OnUnpause;
		public event Action OnPause;

		public override void LeaveState(StatesNames newState)
		{ 
			Background.enabled = true;
			timeScale = GameTime.timeScale;
			GameTime.timeScale = 0f;
			OnPause?.Invoke();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		public override void EnterState()
		{ 
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			GameTime.timeScale = timeScale;
			OnUnpause?.Invoke();
			Background.enabled = false;
		}

		void Awake()
		{
			timeScale = Time.timeScale;
		}
	}
}
