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
		public event Action OnGameOver;

		[SerializeField]private HeroController playerController = null;
		[SerializeField]private HeroController AIController = null;
		[SerializeField]private HeroesDatabase heroesDatabase = null;

		public void InitControllersSet(HeroesNames player, HeroesNames AI)//менять модельки в зависимости от исходных данных
		{
			m_Player = playerController.transform;
			m_AI = AIController.transform;

			playerController.SetHero(player, m_HeroModeDirection, this, heroesDatabase);
			AIController.SetHero(AI, m_HeroModeDirection, this, heroesDatabase);

			m_GameState.OnPause += OnPause;
			m_GameState.OnUnpause += OnUnpause;
			playerController.OnOutOfHP += GameOver;
			AIController.OnOutOfHP += GameOver;
			m_IntroDialog.OnIntroEnded += IntroEnded;
		}

		void IntroEnded()
		{
			playerController.IntroEnded();
			AIController.IntroEnded();
		}

		void FixedUpdate()
		{
			//разворот чтобы контроллеры смотрели друг на друга
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

		void GameOver(HeroController controller)
		{
			if(ReferenceEquals(controller, playerController))
			{
				Debug.Log("Player wins");
			}
			else
			{
				Debug.Log("AI wins");
			}
			OnGameOver?.Invoke();
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
