using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	[RequireComponent(typeof(Control.AICombosRegistration))]
	public class AIController : HeroController
	{
		[Header("Combat logic")]
		[SerializeField]private AICombatLogic m_CombatLogic = null;
		[Header("AI combos registration")]
		[SerializeField] Control.AICombosRegistration combosRegistration = null;

		float vertical = 0f;
		float horizontal = 0f;
		bool Attack = false;
		protected override void ControlLogic()
		{
			m_CombatLogic.CombatAnalysis(out vertical, out horizontal, out Attack, combosRegistration);

			m_ControlFSM.GetInput(vertical, horizontal, Attack);
		}
		public override void SetEnemyData(HeroController enemyController)
		{
			base.SetEnemyData(enemyController);
			m_CombatLogic.SetEnemyData(enemyController);
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
			m_CombosRegistration = combosRegistration;
			m_CombatLogic.LoadAISettings();
			base.m_Awake();
		}
	}
}
