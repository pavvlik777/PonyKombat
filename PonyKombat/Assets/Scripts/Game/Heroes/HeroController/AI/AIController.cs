using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	public class AIController : HeroController
	{
		protected override void ControlLogic()
		{
			//AI Logic

			m_ControlFSM.GetInput(0f, 0f, true);
		}

		void Update()
		{
			ControlLogic();
		}

		void FixedUpdate()
		{
			m_ControlFSM.FixedUpdateState(IsPause, IsIntro);
		}

		void Awake()
		{
			base.m_Awake();
		}
	}
}
