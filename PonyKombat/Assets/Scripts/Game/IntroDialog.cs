using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game
{
	public class IntroDialog : MonoBehaviour
	{
		public event Action OnIntroEnded;
		[SerializeField]private Transform afterIntroCameraPos = null;
		[SerializeField]private Camera m_Camera = null;
		[SerializeField]private Animator m_Animator = null;

		[SerializeField]private n_MenuFSM.GameState m_GameState = null;

		void OnPause()
		{
			m_Animator.enabled = false;
		}

		void OnUnpause()
		{
			m_Animator.enabled = true;
		}

		public void PlayIntro()
		{
			//Debug.Log("Intro started");
			m_GameState.OnPause += OnPause;
			m_GameState.OnUnpause += OnUnpause;
			//IntroEnded();
		}

		void OnDestroy()
		{
			m_GameState.OnPause -= OnPause;
			m_GameState.OnUnpause -= OnUnpause;
		}

		public void IntroEnded()
		{
			m_Camera.transform.position = afterIntroCameraPos.position;
			m_Camera.transform.rotation = afterIntroCameraPos.rotation;
			OnIntroEnded?.Invoke();
			OnIntroEnded = null;
		}
	}
}
