using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	public class WalkState : State
	{
		[Header("State data")]
		[SerializeField]private float jumpHeight = 1f;
		[SerializeField]private float moveSpeed = 3f;

		public override Vector3 LeaveState(StatesNames newState)
		{
			return m_MoveDirection;
		}
		public override void EnterState (Vector3 oldMoveDirection)
		{
			m_MoveDirection = oldMoveDirection;
			isForward = m_Character.localRotation == Quaternion.Euler(0, 90, 0);
			moveSpeed = m_HeroController.HeroStats.moveSpeed;

			m_CharacterController.Move(new Vector3(0f, -m_gravity * GameTime.unscaledFixedDeltaTime, 0f));
		}

		bool isForward = true;
		public override void FixedUpdateState(out Vector3 moveDirection)
		{
			float speed = GetSpeed();
			m_Animator.SetFloat("Speed", speed);
			Vector3 desiredMove = m_HeroMoveDirection.forward * m_Input.y;

			RaycastHit hitInfo;
			Vector3 position0 = new Vector3 (m_Character.localPosition.x, m_Character.localPosition.y - m_CharacterController.height / 2f + m_CharacterController.radius,
				 m_Character.localPosition.z);
			Vector3 position1 = new Vector3 (m_Character.localPosition.x, m_Character.localPosition.y + m_CharacterController.height / 2f - m_CharacterController.radius,
				 m_Character.localPosition.z);
			Physics.CapsuleCast(position0, position1, m_CharacterController.radius, -m_Character.transform.up, out hitInfo, 
				m_CharacterController.radius, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			desiredMove = Vector3.ProjectOnPlane (desiredMove, hitInfo.normal).normalized;

			m_MoveDirection = desiredMove * speed;

			if (m_CharacterController.isGrounded) {
				m_Animator.SetBool("IsInAir", false);
				m_MoveDirection.y -= m_gravity;
				if (vertical == 1) {
					m_Animator.SetTrigger("Jump");

					float time = Mathf.Sqrt (2 * jumpHeight / m_gravity);
					m_MoveDirection.y = m_gravity * time;
					m_Animator.SetBool("IsInAir", true);

					moveDirection = m_MoveDirection;

					m_Animator.ResetTrigger("Jump");
					m_ControlFSM.ChangeState (StatesNames.Air);

					return;
				} else if (vertical == -1) {
					moveDirection = m_MoveDirection;
					//m_ControlFSM.ChangeState (StatesNames.Sit);
					return;
				}
				else if(isAttackPressed)
					{
						m_Animator.SetTrigger("Attack");
						isAttackPressed = false;
						moveDirection = m_MoveDirection;
						m_ControlFSM.ChangeState (StatesNames.Attack);
						return;
					}
			} else {
				m_Animator.SetBool("IsInAir", true);
				moveDirection = m_MoveDirection;
				m_ControlFSM.ChangeState (StatesNames.Air);
				return;
			}
			
			moveDirection = m_MoveDirection;
			if(m_Enemy.localPosition.x > m_Character.localPosition.x)
				m_Character.localRotation = Quaternion.Euler(0, 90, 0);
			else
				m_Character.localRotation = Quaternion.Euler(0, -90, 0);
			isForward = m_Character.localRotation == Quaternion.Euler(0, 90, 0);

			if(isForward)
				m_Animator.SetBool("IsForward", horizontal > 0f);
			else
				m_Animator.SetBool("IsForward", horizontal < 0f);
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
