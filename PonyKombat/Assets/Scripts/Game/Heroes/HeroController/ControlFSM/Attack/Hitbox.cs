using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.Control
{
	[RequireComponent(typeof(Collider))]
	public class Hitbox : MonoBehaviour
	{
		private Collider m_Collider;
		private float damage;
		private bool isCanDamage = false;

		void Awake()
		{
			m_Collider = GetComponent<Collider>();
		}

		public void InitSet(float _damage)
		{
			SetActive(false);
			damage = _damage;
		}

		public void SetActive(bool state)
		{
			m_Collider.enabled = state;
			isCanDamage = state;
		}

		void OnTriggerEnter(Collider other)
		{
			if(!isCanDamage)
				return;
			isCanDamage = false;
			Hurtbox _other = other.GetComponent<Hurtbox>();
			_other.GetDamage(damage);
		}
	}
}
