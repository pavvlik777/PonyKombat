using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Rigidbody))]
	public class Hurtbox : MonoBehaviour
	{
		[SerializeField]private Collider m_Collider = null;
		public event Action<float> OnHitted;
		void OnTriggerEnter(Collider other)
		{
			
		}

		public void GetDamage(float value)
		{
			OnHitted?.Invoke(value);
		}

		public void SetActive(bool state)
		{
			m_Collider.enabled = state;
		}
	}
}
