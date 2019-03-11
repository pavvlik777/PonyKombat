using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	[System.Serializable]
	public class Hero
	{
		public HeroesNames heroName = HeroesNames.Applejack;

		public float moveSpeed = 5f;
		public float maxHP = 100f;

		public float attackDamage = 5f;

		public Hero(float _moveSpeed, float _maxHP, float _attackDamage, HeroesNames _name)
		{
			moveSpeed = _moveSpeed;
			maxHP = _maxHP;
			heroName = _name;
			attackDamage = _attackDamage;
		}

		public Hero(Hero other) : this(other.moveSpeed, other.maxHP, other.attackDamage, other.heroName)
		{ }

		public Hero() : this(5f, 100f, 5f, HeroesNames.Applejack)
		{ }
	}
}
