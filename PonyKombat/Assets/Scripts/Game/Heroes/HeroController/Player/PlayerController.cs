using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	public class PlayerController : HeroController
	{
		protected override void ControlLogic()
		{
			m_ControlFSM.GetInput(GameInput.GetAxis("Vertical"), GameInput.GetAxis("Horizontal"), GameInput.GetButton("X"));
		}

		void Update()
		{
			ControlLogic();
		}

		void FixedUpdate()
		{
			m_ControlFSM.FixedUpdateState(IsPause);
		}

		void Awake()
		{
			base.m_Awake();
		}
	}
}
