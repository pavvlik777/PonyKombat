using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	public class CombatStateController : MonoBehaviour
	{
		[SerializeField]private n_MenuFSM.GameState m_GameState = null;
		[SerializeField]private UI.GameUI m_GameUI = null;
		[SerializeField]private IntroDialog m_IntroDialog = null;
		[SerializeField]private AudioListener m_GameListener = null;
		[SerializeField]private AudioListener m_MenuListener = null;

		[SerializeField]private Transform m_HeroModeDirection = null;
		private Transform m_Player;
		private Transform m_AI;

		public event Action OnGamePaused;
		public event Action OnGameUnpaused;
		public event Action<int, int, int> OnGameOver;

		[SerializeField]private HeroController playerController = null;
		[SerializeField]private HeroController AIController = null;

		public void InitControllersSet(HeroesNames player, HeroesNames AI)//менять модельки в зависимости от исходных данных
		{
			m_Player = playerController.transform;
			m_AI = AIController.transform;

			playerController.SetHero(player, m_HeroModeDirection, this);
			AIController.SetHero(AI, m_HeroModeDirection, this);

			m_GameState.OnPause += OnPause;
			m_GameState.OnUnpause += OnUnpause;
			playerController.OnOutOfHP += GameOver;
			AIController.OnOutOfHP += GameOver;
			m_IntroDialog.OnIntroEnded += IntroEnded;
			GameConsole.OnUserCommand += UserCommandsReaction;
		}

		void UserCommandsReaction(string command) //add logic
		{
			switch(command)
			{
				case "g_restore_hp_player":
					playerController.RestoreHP();
				break;
				case "g_restore_hp_ai":
					AIController.RestoreHP();
				break;
			}
		}

		void IntroEnded()
		{
			playerController.IntroEnded();
			AIController.IntroEnded();
		}

		void FixedUpdate()
		{
			
		}

		void OnPause()
		{
			m_GameUI.HideUI();
			m_GameListener.enabled = false;
			m_MenuListener.enabled = true;
			OnGamePaused?.Invoke();
		}
		void OnUnpause()
		{
			m_GameUI.ShowUI();
			m_GameListener.enabled = true;
			m_MenuListener.enabled = false;
			OnGameUnpaused?.Invoke();
		}

		void GameOver(HeroController controller, bool isSomebodyWon)
		{
			if(isSomebodyWon)
			{
				if(ReferenceEquals(controller, playerController))
				{
					Debug.Log("AI wins");
					OnGameOver?.Invoke(1, 0, 1);
				}
				else
				{
					Debug.Log("Player wins");
					OnGameOver?.Invoke(0, 0, -1);
				}
			}
			else
			{
				Debug.Log("Draw");
				OnGameOver?.Invoke(0, 0, 0);
			}
		}

		void OnDestroy()
		{
			m_GameState.OnPause -= OnPause;
			m_GameState.OnUnpause -= OnUnpause;
			playerController.OnOutOfHP -= GameOver;
			AIController.OnOutOfHP -= GameOver;
			m_IntroDialog.OnIntroEnded -= IntroEnded;
		}
	}
}
