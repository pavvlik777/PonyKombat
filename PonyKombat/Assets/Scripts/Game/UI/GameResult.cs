using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_Game.Combat.UI
{
	public class GameResult : MonoBehaviour
	{
		[SerializeField]private GameObject rootObject = null;
		[SerializeField]private Text resultMessage = null;

		[SerializeField]private float timeMessageActive = 5f;

		public void ShowMessage(string s)
		{
			resultMessage.text = s;
			rootObject.SetActive(true);
			StartCoroutine(GoToMainMenu());
		}

		IEnumerator GoToMainMenu()
		{
			float timePassed = 0f;
			while(timePassed <= timeMessageActive)
			{
				timePassed += GameTime.deltaTime;
				yield return null;
			}
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
			yield break;
		}
	}
}
