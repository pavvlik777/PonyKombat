using System;
using UnityEngine;
using UnityEngine.UI;
using n_Game;

namespace n_MenuFSM
{
	public class NewGameState : State
	{
		[SerializeField]private MenuFSM menu = null;
		[Header("Choose hero data")]
		[SerializeField]private Sprite UnknownHeroSprite = null;
		[SerializeField]private Image PlayerHeroImage = null;
		[SerializeField]private Image AIHeroImage = null;

		[SerializeField]private Color defaultColor = Color.blue;
		[SerializeField]private Color playerOverColor = Color.green;
		[SerializeField]private Color aiOverColor = Color.red;

		private bool isPlayerCanChoose = true;

		[Header("Awailable heroes list")]
		[SerializeField]private ChooseHero[] heroes = null;
		private ChooseHero currentPlayerHero = null;
		private ChooseHero currentAIHero = null;

		[Header("Player warning message box")]
		[SerializeField]private GameObject messageBox = null;

		public override void EnterState()
		{ 
			Awake();
			SwitchStateObject (true);
		}

		void Awake()
		{
			isPlayerCanChoose = true;
			PlayerHeroImage.sprite = UnknownHeroSprite;
			AIHeroImage.sprite = UnknownHeroSprite;
			currentPlayerHero = null;
			currentAIHero = null;
			foreach(var cur in heroes)
			{
				cur.HidePlayerImages();
				cur.SetBorderColor(defaultColor);
				cur.OnClick += ChangePlayerHero;
				cur.OnMouseEnter += SetEnterColor;
				cur.OnMouseExit += ResetEnterColors;
			}
			ChooseAIHero();
			HideWarningMessageBox();
		}

		void ChooseAIHero()
		{
			foreach(var cur in heroes)
			{
				if(cur.HeroName == HeroesNames.Random)
				{
					if(currentAIHero != null)
						currentAIHero.SetPlayer(2, false);
					cur.SetPlayer(2, true);
					currentAIHero = cur;

					AIHeroImage.sprite = currentAIHero.HeroSprite;
				}
			}
		}

		void ChangePlayerHero(ChooseHero hero)
		{
			if(!isPlayerCanChoose) return;
			if(currentPlayerHero != null)
				currentPlayerHero.SetPlayer(1, false);
			hero.SetPlayer(1, true);
			currentPlayerHero = hero;

			PlayerHeroImage.sprite = currentPlayerHero.HeroSprite;
		}

		void SetEnterColor(ChooseHero hero)
		{
			if(!isPlayerCanChoose) return;
			hero.SetBorderColor(playerOverColor);
		}

		void ResetEnterColors(ChooseHero hero)
		{
			if(!isPlayerCanChoose) return;
			hero.SetBorderColor(defaultColor);
		}

		public void StartGame()
		{
			if(currentPlayerHero == null)
			{
				ShowWarningMessageBox();
				return;
			}
			LoadChosenHero.chosenPlayerHero = currentPlayerHero.HeroName;
			LoadChosenHero.chosenAIHero = currentAIHero.HeroName;
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("FightLevel");
		}

		void ShowWarningMessageBox()
		{
			messageBox.SetActive(true);
			menu.LockChangeState();
		}

		public void HideWarningMessageBox()
		{
			messageBox.SetActive(false);
			menu.UnlockChangeState();
		}
	}
}
