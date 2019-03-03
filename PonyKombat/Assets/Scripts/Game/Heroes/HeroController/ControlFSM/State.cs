using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public enum StatesNames
	{ Walk, Air, Attack, HitReaction }

	public abstract class State : MonoBehaviour
	{
		protected float vertical;
		protected float horizontal;
		protected bool isAttackPressed;

		protected Transform m_Character;
		protected CharacterController m_CharacterController;

		[SerializeField]protected StatesNames stateName = StatesNames.Walk;
		public StatesNames StateName
		{
			get { return stateName; }
		}

		public void StateInitialization()
		{

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
