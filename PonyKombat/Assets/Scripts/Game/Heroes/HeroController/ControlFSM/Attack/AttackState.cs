using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class AttackState : State
	{
		[SerializeField]private Hitbox[] hitboxes = null;

		public override Vector3 LeaveState(StatesNames newState)
		{
			return Vector3.zero;
		}
		public override void EnterState (Vector3 oldMoveDirection)
		{
			foreach(var cur in hitboxes)
				cur.InitSet(m_HeroController.AttackDamage);
			m_MoveDirection.y = oldMoveDirection.y;
		}
		public override void FixedUpdateState(out Vector3 moveDirection)
		{
			moveDirection = Vector3.zero;
		}

		public void AttackStarted()
		{
			foreach(var cur in hitboxes)
				cur.SetActive(true);
		}

		public void AttackFinished()
		{
			foreach(var cur in hitboxes)
				cur.SetActive(false);
		}

		public void StateFinished()
		{
			m_ControlFSM.ChangeState(StatesNames.Walk);
		}
	}
}
