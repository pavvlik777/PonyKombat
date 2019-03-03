using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class WalkState : State
	{
		public override Vector3 LeaveState(StatesNames newState)
		{
			return Vector3.zero;
		}
		public override void EnterState (Vector3 oldMoveDirection)
		{

		}
		public override void FixedUpdateState(out Vector3 moveDirection)
		{
			moveDirection = Vector3.zero;
			if(horizontal > 0)
				moveDirection = new Vector3(1, 0 ,0);
			else if(horizontal < 0)
				moveDirection = new Vector3(-1, 0 ,0);

		}
	}
}
