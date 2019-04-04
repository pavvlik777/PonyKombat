using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class AttackState : State
	{
		[Serializable]
		private struct AttackData
		{
			[SerializeField]private List<Hitbox> hitboxes;

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

			public void InitSet(float damage)
			{
				foreach(var cur in hitboxes)
					cur.InitSet(damage);
			}

			public AttackData(List<Hitbox> init)
			{
				hitboxes = init;
			}
		}
		
		[SerializeField]private AttackData[] attacks = null;

		private int currentAttack = 0;
		public override Vector3 LeaveState(StatesNames newState)
		{
			return Vector3.zero;
		}
		public override void EnterState (Vector3 oldMoveDirection)
		{
			currentAttack = (int)m_Animator.GetFloat("CurrentAttack");
			attacks[currentAttack].InitSet(m_HeroController.AttackDamage);
			m_MoveDirection.y = oldMoveDirection.y;
		}
		public override void FixedUpdateState(out Vector3 moveDirection)
		{
			moveDirection = Vector3.zero;
		}

		public void AttackStarted()
		{
			attacks[currentAttack].AttackStarted();
		}

		public void AttackFinished()
		{
			attacks[currentAttack].AttackFinished();
			m_CombosRegistration.ResetCombo();
		}

		public void StateFinished()
		{
			m_ControlFSM.ChangeState(StatesNames.Walk);
		}
	}
}
