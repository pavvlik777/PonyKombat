using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat
{
	public class HeroesDatabase : MonoBehaviour //Singleton
	{
		[SerializeField]private List<Hero> Heroes = null;

		public Hero this[int i]
		{
			get { return Heroes[i]; }
		}

		public Hero this[HeroesNames s]
		{
			get
			{
				if(s == HeroesNames.Random)
				{
					int n = UnityEngine.Random.Range(0, Heroes.Count);
					return Heroes[n];
				}
				foreach(var cur in Heroes)
					if(cur.heroName == s)
						return cur;
				throw new ArgumentException("There are no hero with such name");
			}
		}

		public static HeroesDatabase instance = null;
		void Awake()
		{
			if(instance && instance != this)
			{
				Debug.LogError("Another instance of heroes database");
				Destroy(this);
				return;
			}
			instance = this;
		}
	}
}
