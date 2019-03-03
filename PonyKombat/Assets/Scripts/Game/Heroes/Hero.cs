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

		public Hero(float _moveSpeed, float _maxHP, HeroesNames _name)
		{
			moveSpeed = _moveSpeed;
			maxHP = _maxHP;
			heroName = _name;
		}

		public Hero(Hero other) : this(other.moveSpeed, other.maxHP, other.heroName)
		{ }

		public Hero() : this(5f, 100f, HeroesNames.Applejack)
		{ }
	}
}
