using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	[RequireComponent(typeof(Control.PlayerCombosRegistration))]
	public class PlayerController : HeroController
	{
		[Header("Player combos registration")]
		[SerializeField] Control.PlayerCombosRegistration combosRegistration = null;

		protected override void ControlLogic()
		{
			m_ControlFSM.GetInput(GameInput.GetAxis("Vertical"), GameInput.GetAxis("Horizontal"), GameInput.GetButton("X") || GameInput.GetButton("Y") 
				|| GameInput.GetButton("A") || GameInput.GetButton("B"));
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
			base.m_Awake();
		}
	}
}
