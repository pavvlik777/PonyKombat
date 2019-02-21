using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	public enum HeroesNames
	{ Applejack, Fluttershy, Unknown, Random }

	public class ChooseHero : MonoBehaviour
	{
		public event Action<ChooseHero> OnClick;
		public event Action<ChooseHero> OnMouseEnter;
		public event Action<ChooseHero> OnMouseExit;

		[SerializeField]private Image borderImage = null;
		[SerializeField]private Image player1Image = null;
		[SerializeField]private Image player2Image = null;

		[SerializeField]private HeroesNames heroName = HeroesNames.Applejack;
		[SerializeField]private Sprite heroSprite = null;
		public HeroesNames HeroName 
		{
			get 
			{
				return heroName;
			}
		}
		public Sprite HeroSprite 
		{
			get 
			{
				return heroSprite;
			}
		}

		public void HidePlayerImages()
		{
			player1Image.enabled = false;
			player2Image.enabled = false;
		}

		public void SetBorderColor(Color color)
		{
			borderImage.color = color;
		}

		public void InitChangeColorOnEnter()
		{
			OnMouseEnter?.Invoke(this);
		}

		public void InitChangeColorOnExit()
		{
			OnMouseExit?.Invoke(this);
		}

		public void InitHeroClick()
		{
			OnClick?.Invoke(this);
		}

		public void SetPlayer(int number, bool status)
		{
			if(number == 1)
				player1Image.enabled = status;
			else if(number == 2)
				player2Image.enabled = status;
			else throw new ArgumentOutOfRangeException("Wrong player number");
		}
	}
}
