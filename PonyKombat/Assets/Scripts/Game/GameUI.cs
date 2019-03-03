using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.UI
{
	public class GameUI : MonoBehaviour
	{
		private Canvas m_Canvas;

		void Awake()
		{
			m_Canvas = GetComponent<Canvas>();
		}

		public void ShowUI()
		{
			m_Canvas.enabled = true;
		}

		public void HideUI()
		{
			m_Canvas.enabled = false;
		}
	}
}
