using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class HitState : State
	{
		private Vector3 initMoveDirection;
		public override Vector3 LeaveState(StatesNames newState)
		{
			return m_MoveDirection;
		}
		public override void EnterState (Vector3 oldMoveDirection)
		{
			initMoveDirection = oldMoveDirection;
			m_MoveDirection.y = oldMoveDirection.y;
		}
		public override void FixedUpdateState(out Vector3 moveDirection)
		{
			m_MoveDirection.x = initMoveDirection.x;
			m_MoveDirection.z = initMoveDirection.z;
			if (m_CharacterController.isGrounded) {
				m_Animator.SetBool("IsInAir", false);
				moveDirection = m_MoveDirection;
				// m_SoundsController.PlayLandSound();
				return;
			} else {
				m_MoveDirection.y -= m_gravity * GameTime.fixedDeltaTime;
			}

			moveDirection = m_MoveDirection;
		}

		public void HitFinished()
		{
			m_ControlFSM.ChangeState(StatesNames.Walk);
		}
	}
}
