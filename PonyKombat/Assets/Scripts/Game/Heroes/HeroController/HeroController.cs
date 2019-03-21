﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using n_Game.Combat.Control;

namespace n_Game.Combat
{
	public abstract class HeroController : MonoBehaviour
	{
		protected CombatStateController combatController;
		protected CharacterController m_CharacterController;
		protected Transform moveDirection;
		protected Animator m_Animator;
		protected bool IsIntro = false;
		protected bool IsPause = false;

		protected Hero heroStats;
		protected Hero currentHeroStats;

		protected Vector3 initPosition;

		public float AttackDamage
		{ get { return heroStats.attackDamage; } }
		public HeroesNames HeroName
		{ get { return heroStats.heroName; } }

		public event Action<HeroController, bool> OnOutOfHP;

		[Header("HP slider")]
		[SerializeField]protected Slider HPSlider = null;

		[Header("Moving FSM components")]
		[SerializeField]protected ControlFSM m_ControlFSM = null;
		[SerializeField]protected Hurtbox hurtbox = null;

		[Header("Enemy transorm")]
		[SerializeField]private Transform m_Enemy = null;

		public void SetHero(HeroesNames heroName, Transform moveDirection, CombatStateController _controller)
		{
			this.moveDirection = moveDirection;
			combatController = _controller;
			combatController.OnGamePaused += OnPause;
			combatController.OnGameUnpaused += OnUnpause;

			heroStats = new Hero(HeroesDatabase.instance[heroName]);
			currentHeroStats = new Hero(heroStats);

			initPosition = transform.position;

			SetHPSlider();
			hurtbox.OnHitted += DecreaseHP;
		}
		public void IntroStarted()
		{
			IsIntro = true;
		}
		public void IntroEnded()
		{
			IsIntro = false;
		}
		protected void OnPause()
		{
			IsPause = true;
			m_Animator.enabled = false;
		}
		protected void OnUnpause()
		{
			IsPause = false;
			m_Animator.enabled = true;
		}

		public void DecreaseHP(float amount)
		{
			if(currentHeroStats.maxHP <= 0 || IsIntro)
				return;

			currentHeroStats.maxHP -= amount;
			if(currentHeroStats.maxHP <= 0)
				OnOutOfHP?.Invoke(this, true);
			RefreshHPSlider();
		}

		public void RestoreHP()
		{
			currentHeroStats.maxHP = heroStats.maxHP;
			RefreshHPSlider();
		}

		public void RestoreStartState()
		{
			m_CharacterController.enabled = false;
			transform.position = initPosition;
			m_CharacterController.enabled = true;
			m_ControlFSM.Restore();
			RestoreHP();
		}

		void SetHPSlider()
		{
			HPSlider.maxValue = heroStats.maxHP;
			HPSlider.value = currentHeroStats.maxHP;
		}
		void RefreshHPSlider()
		{
			HPSlider.value = currentHeroStats.maxHP;
		}

		protected void m_Awake()
		{
			m_ControlFSM = GetComponent<ControlFSM>();
			m_Animator = GetComponent<Animator>();
			m_CharacterController = GetComponent<CharacterController>();
			m_ControlFSM.FSMInitialization(m_Enemy, moveDirection, this);
		}

		protected abstract void ControlLogic();
	}
}
