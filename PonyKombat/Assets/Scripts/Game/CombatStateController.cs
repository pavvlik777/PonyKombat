﻿using System;
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
		[SerializeField]private Music.GameMusicController musicController = null;
		[SerializeField]private n_MenuFSM.MenuFSM m_MenuFSM = null;
		[SerializeField]private Music.AnnouncementController m_Announcement = null;

		[SerializeField]private Transform m_HeroModeDirection = null;
		[SerializeField]private int amountOfRound = 2;
		private int currentRound = 0;
		private int playerPoints = 0;
		private int aiPoints = 0;
		private Transform m_Player;
		private Transform m_AI;

		public event Action OnGamePaused;
		public event Action OnGameUnpaused;
		public event Action<int, int, int> OnRoundOver;
		public event Action OnGameOver;

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

			m_GameUI.OnClockEnded += GameOver;

			IntroStarted();
		}

		void Start()
		{
			musicController.PlayMusic(playerController.HeroName);
			m_IntroDialog.PlayIntro();
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

		void IntroStarted()
		{
			playerController.IntroStarted();
			AIController.IntroStarted();
		}
		void IntroEnded()
		{
			playerController.IntroEnded();
			AIController.IntroEnded();
			m_Announcement.PlayRoundSound(currentRound);
			m_GameUI.ShowMessage($"Round {currentRound + 1}");
			m_GameUI.StartClock(99f);
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

		void RestoreGame()
		{
			playerController.RestoreStartState();
			AIController.RestoreStartState();
			playerController.IntroEnded();
			AIController.IntroEnded();
			m_Announcement.PlayRoundSound(currentRound);
			m_GameUI.ShowMessage($"Round {currentRound + 1}");
			m_GameUI.StartClock(99f);
		}

		void GameOver(HeroController controller, bool isSomebodyWon)
		{
			IntroStarted();
			if(isSomebodyWon)
			{
				m_GameUI.StopClock();
				if(ReferenceEquals(controller, playerController))
				{
					Debug.Log("AI wins");
					aiPoints++;
					OnRoundOver?.Invoke(1, currentRound, 1);
				}
				else
				{
					Debug.Log("Player wins");
					playerPoints++;
					OnRoundOver?.Invoke(0, currentRound, -1);
				}
			}
			else
			{
				Debug.Log("Draw");
				OnRoundOver?.Invoke(0, currentRound, 0);
			}
			currentRound++;
			if(currentRound < amountOfRound)
			{
				RestoreGame();
			}
			else
			{
				OnGameOver?.Invoke();
				m_MenuFSM.LockChangeState();
				if(playerPoints > aiPoints)
				{
					m_Announcement.PlayHeroWin(playerController.HeroName);
					m_GameUI.ShowGameResult($"{playerController.HeroName} wins!");
				}
				else if(playerPoints < aiPoints)
				{
					m_Announcement.PlayHeroWin(AIController.HeroName);
					m_GameUI.ShowGameResult($"{AIController.HeroName} wins!");
				}
				else
					m_GameUI.ShowGameResult("Draw!");
			}
		}

		void OnDestroy()
		{
			m_GameState.OnPause -= OnPause;
			m_GameState.OnUnpause -= OnUnpause;
			playerController.OnOutOfHP -= GameOver;
			AIController.OnOutOfHP -= GameOver;
			m_IntroDialog.OnIntroEnded -= IntroEnded;
			m_GameUI.OnClockEnded -= GameOver;
		}
	}
}
