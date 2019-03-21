using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class HitState : State
	{
		public override Vector3 LeaveState(StatesNames newState)
		{
			return Vector3.zero;
		}
		public override void EnterState (Vector3 oldMoveDirection)
		{
			m_MoveDirection.y = oldMoveDirection.y;
		}
		public override void FixedUpdateState(out Vector3 moveDirection)
		{
			moveDirection = Vector3.zero;
		}

		public void HitFinished()
		{
			m_ControlFSM.ChangeState(StatesNames.Walk);
		}
	}
}
