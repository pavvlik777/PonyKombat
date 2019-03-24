using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	public class AIController : HeroController
	{
		[Header("Combat logic")]
		[SerializeField]private AICombatLogic m_CombatLogic = null;

		float vertical = 0f;
		float horizontal = 0f;
		bool Attack = false;
		protected override void ControlLogic()
		{
			m_CombatLogic.CombatAnalysis(out vertical, out horizontal, out Attack);

			m_ControlFSM.GetInput(vertical, horizontal, Attack);
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
			m_CombatLogic.LoadAISettings();
			base.m_Awake();
		}
	}
}
