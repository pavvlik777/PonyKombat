using System;
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
		protected Animator m_Animator;
		protected bool IsPause = false;

		protected Hero heroStats;
		protected Hero currentHeroStats;

		public event Action<HeroController> OnOutOfHP;

		[Header("HP slider")]
		[SerializeField]protected Slider HPSlider = null;

		[Header("Moving FSM components")]
		[SerializeField]protected ControlFSM m_ControlFSM;

		public void SetHero(HeroesNames heroName, CombatStateController _controller, HeroesDatabase _database)
		{
			combatController = _controller;
			IsPause = false;
			combatController.OnGamePaused += OnPause;
			combatController.OnGameUnpaused += OnUnpause;

			heroStats = new Hero(_database[heroName]);
			currentHeroStats = new Hero(heroStats);

			SetHPSlider();
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
			currentHeroStats.maxHP -= amount;
			if(currentHeroStats.maxHP <= 0)
				OnOutOfHP?.Invoke(this);
			RefreshHPSlider();
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
		}

		protected abstract void ControlLogic();
	}
}
