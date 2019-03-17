using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game
{
	public class IntroDialog : MonoBehaviour
	{
		public event Action OnIntroEnded;
		[SerializeField]private Camera m_Camera = null;

		public void PlayIntro()
		{
			Debug.Log("Intro started");
			IntroEnded();
		}

		public void IntroEnded()
		{
			//reset cameraPos
			OnIntroEnded?.Invoke();
			OnIntroEnded = null;
		}
	}
}
