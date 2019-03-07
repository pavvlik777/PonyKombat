using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class JumpState : State
	{
		[Header("State data")]
		[SerializeField]private float moveSpeed = 0.5f;
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
			float speed = GetSpeed ();
			Vector3 desiredMove = m_HeroMoveDirection.forward * m_Input.y + m_Character.right * m_Input.x;

			RaycastHit hitInfo;
			Vector3 position0 = new Vector3 (m_Character.localPosition.x, m_Character.localPosition.y - m_CharacterController.height / 2f + m_CharacterController.radius,
				 m_Character.localPosition.z);
			Vector3 position1 = new Vector3 (m_Character.localPosition.x, m_Character.localPosition.y + m_CharacterController.height / 2f - m_CharacterController.radius,
				 m_Character.localPosition.z);
			Physics.CapsuleCast(position0, position1, m_CharacterController.radius, -m_Character.transform.up, out hitInfo, 
				m_CharacterController.radius, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			desiredMove = Vector3.ProjectOnPlane (desiredMove, hitInfo.normal).normalized;

			m_MoveDirection.x = initMoveDirection.x + desiredMove.x * speed;
			m_MoveDirection.z = initMoveDirection.z + desiredMove.z * speed;

			if (m_CharacterController.isGrounded) {
				m_Animator.SetBool("IsInAir", false);
				moveDirection = m_MoveDirection;
				m_ControlFSM.ChangeState (StatesNames.Walk);
				// m_SoundsController.PlayLandSound();
				return;
			} else {
				m_MoveDirection.y -= m_gravity * GameTime.fixedDeltaTime;
			}

			moveDirection = m_MoveDirection;
		}

		float GetSpeed()
		{
			m_Input = new Vector2(0, horizontal);

			if (m_Input.sqrMagnitude > 1)
			{
				m_Input.Normalize();
			}

			float speed = horizontal == 0 ? 0f : moveSpeed;
			return speed;
		}
	}
}
