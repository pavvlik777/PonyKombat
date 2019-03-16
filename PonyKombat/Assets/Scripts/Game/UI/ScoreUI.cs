using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Game.Combat.UI
{
	public class ScoreUI : MonoBehaviour
	{
		[SerializeField] private ScorePoint[] playerPoints = null;
		[SerializeField] private ScorePoint[] aiPoints = null;

		[SerializeField] private Color winColor = Color.green;
		[SerializeField] private Color loseColor = Color.red;
		[SerializeField] private Color drawColor = Color.blue;

		[SerializeField]private CombatStateController m_Controller = null;

		void Awake()
		{
			m_Controller.OnGameOver += UpdateScore;
		}

		void UpdateScore(int player, int round, int result)
		{
			if(result != 0)
			{
				if(player == 0)
				{
					playerPoints[round].SetValue(winColor);
					aiPoints[round].SetValue(loseColor);
				}
				else if(player == 1)
				{
					playerPoints[round].SetValue(loseColor);
					aiPoints[round].SetValue(winColor);
				}
				else
					throw new ArgumentOutOfRangeException("max 2 player");
			}
			else
			{
				playerPoints[round].SetValue(drawColor);
				aiPoints[round].SetValue(drawColor);
			}
		}
	}
}
