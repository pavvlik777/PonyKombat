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

		public event Action OnIntroEnded;
		public event Action OnGamePaused;
		public event Action OnGameUnpaused;
		public event Action OnGameOver;

		[SerializeField]private HeroController playerController = null;
		[SerializeField]private HeroController AIController = null;
		[SerializeField]private HeroesDatabase heroesDatabase;

		public void InitControllersSet(HeroesNames player, HeroesNames AI)//менять модельки в зависимости от исходных данных
		{
			playerController.SetHero(player, this, heroesDatabase);
			AIController.SetHero(AI, this, heroesDatabase);

			m_GameState.OnPause += OnPause;
			m_GameState.OnUnpause += OnUnpause;
			playerController.OnOutOfHP += GameOver;
			AIController.OnOutOfHP += GameOver;
		}

		void OnPause()
		{
			m_GameUI.HideUI();
			OnGamePaused?.Invoke();
		}
		void OnUnpause()
		{
			m_GameUI.ShowUI();
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
		}

		void OnDestroy()
		{
			m_GameState.OnPause -= OnPause;
			m_GameState.OnUnpause -= OnUnpause;
			playerController.OnOutOfHP -= GameOver;
			AIController.OnOutOfHP -= GameOver;
		}
	}
}
