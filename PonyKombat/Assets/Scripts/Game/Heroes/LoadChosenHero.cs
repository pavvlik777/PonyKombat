using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_Game.Combat;

namespace n_Game
{
	public class LoadChosenHero : MonoBehaviour
	{
		public static HeroesNames chosenPlayerHero = HeroesNames.Unknown;
		public static HeroesNames chosenAIHero = HeroesNames.Unknown;

		[SerializeField]private bool isSceneDebug = true;
		[SerializeField]private HeroesNames debugHero = HeroesNames.Random;

		[SerializeField]private CombatStateController combatController = null;

		void Awake()
		{
			if(isSceneDebug)
				LoadInitHeroes();
			SetHeroes();
		}

		void LoadInitHeroes()
		{
			chosenPlayerHero = HeroesNames.PinkiePie;
			chosenAIHero = debugHero;
		}

		void SetHeroes()
		{
			combatController.InitControllersSet(chosenPlayerHero, chosenAIHero);
		}
	}
}
