using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game
{
	public class LoadChosenHero : MonoBehaviour
	{
		public static HeroesNames chosenPlayerHero = HeroesNames.Unknown;
		public static HeroesNames chosenAIHero = HeroesNames.Unknown;

		[SerializeField]private bool isSceneDebug = true;

		void Awake()
		{
			if(isSceneDebug)
				LoadInitHeroes();
		}

		void LoadInitHeroes()
		{
			chosenPlayerHero = HeroesNames.Applejack;
			chosenAIHero = HeroesNames.Random;
		}
	}
}
