using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public enum StatesNames
	{ Walk, Air, Attack, HitReaction, Sit }

	public abstract class State : MonoBehaviour
	{
		protected float vertical;
		protected float horizontal;
		protected bool isAttackPressed;

		protected Transform m_HeroMoveDirection;
		protected Transform m_Character;
		protected Transform m_Enemy;
		protected CharacterController m_CharacterController;
		protected Animator m_Animator;
		protected ControlFSM m_ControlFSM;
		protected HeroController m_HeroController;

		protected Vector3 m_MoveDirection;
		protected Vector2 m_Input;

		[Header("State name")]
		[SerializeField]protected StatesNames stateName = StatesNames.Walk;
		public StatesNames StateName
		{ get { return stateName; } }

		[Header("Common states data")]
		[SerializeField]protected float m_gravity = 9.8f;

		public void StateInitialization(Transform enemy, Transform moveDirection, Transform character, CharacterController controller, Animator animator, ControlFSM controlFSM, HeroController heroController)
		{
			m_Enemy = enemy;
			m_HeroMoveDirection = moveDirection;
			m_Character = character;
			m_CharacterController = controller;
			m_Animator = animator;
			m_ControlFSM = controlFSM;
			m_HeroController = heroController;
		}
		public void ButtonsCheckUpdate (float vertical, float horizontal, bool isAttackPressed)
		{
			this.vertical = vertical;
			this.horizontal = horizontal;
			this.isAttackPressed = isAttackPressed;
		}

		public abstract Vector3 LeaveState(StatesNames newState);
		public abstract void EnterState (Vector3 oldMoveDirection);
		public abstract void FixedUpdateState(out Vector3 moveDirection);
	}
}
