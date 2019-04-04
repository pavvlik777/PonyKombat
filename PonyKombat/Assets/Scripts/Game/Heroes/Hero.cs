using System;
using System.Collections.Generic;

namespace n_Game.Combat
{
	[System.Serializable]
	public class Hero
	{
		public HeroesNames heroName = HeroesNames.Applejack;

		public float moveSpeed = 5f;
		public float maxHP = 100f;

		public Dictionary<string, float> combos = null;

		public Hero(float _moveSpeed, float _maxHP, Dictionary<string, float> _combos, HeroesNames _name)
		{
			moveSpeed = _moveSpeed;
			maxHP = _maxHP;
			heroName = _name;
			combos = _combos;
		}

		public Hero(Hero other) : this(other.moveSpeed, other.maxHP, other.combos, other.heroName)
		{ }

		public Hero() : this(5f, 100f, new Dictionary<string, float>{ }, HeroesNames.Applejack)
		{ }
	}
}
