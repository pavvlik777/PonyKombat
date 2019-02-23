using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_MenuFSM
{
	public class ExitGameState : State
	{
		public void AcceptExitGame()
		{
			Application.Quit();
		}

		public void AcceptExitCurrentGame()
		{
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
		}
	}
}
