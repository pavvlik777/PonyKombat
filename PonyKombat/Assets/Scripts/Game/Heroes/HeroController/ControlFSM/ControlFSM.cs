﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class ControlFSM : MonoBehaviour
	{
		private Transform m_Character;

		private State currentState;
		[SerializeField]private StatesNames initState = StatesNames.Walk;
		[SerializeField]private State[] statesList = null;
		
		private CharacterController m_CharacterController;
		private Animator m_Animator;
		private HeroController m_HeroController;

		private CollisionFlags m_CollisionFlags;
		private Vector3 m_MoveDirection;

		State this[StatesNames stateName]
		{
			get
			{
				foreach (State cur in statesList)
					if (cur.StateName == stateName)
						return cur;
				throw new System.ArgumentOutOfRangeException ();
			}
		}

		public void GetInput(float vertical, float horizontal, bool isAttackPressed)
		{
			currentState.ButtonsCheckUpdate(vertical, horizontal, isAttackPressed);
		}

		public void FixedUpdateState(bool IsPause, bool IsIntro)
		{
			if(IsPause || IsIntro)
				return;
			currentState.FixedUpdateState(out m_MoveDirection);

			m_CollisionFlags = m_CharacterController.Move(m_MoveDirection * GameTime.fixedDeltaTime);
		}

		public void FSMInitialization(Transform enemy, Transform moveDirection, HeroController heroController)
		{
			m_Character = transform;
			m_CharacterController = GetComponent<CharacterController>();
			m_Animator = GetComponent<Animator>();
			m_HeroController = heroController;

			foreach(var cur in statesList)
				cur.StateInitialization(enemy, moveDirection, m_Character, m_CharacterController, m_Animator, this, m_HeroController);
			currentState = this[initState];
			currentState.EnterState(m_MoveDirection);
		}

		public void ChangeState(StatesNames newState)
		{
			Vector3 m_MoveDirection = currentState.LeaveState(newState);
			currentState = this[newState];
			currentState.EnterState(m_MoveDirection);
		}
	}
}
