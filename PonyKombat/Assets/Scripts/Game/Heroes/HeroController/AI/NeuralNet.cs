using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace n_Game.Combat.NeuralNet
{
	public class NeuralNet : MonoBehaviour
	{
		[SerializeField]private int numberOfPlayer = -1; // -1 - first, 1 - second
		[SerializeField]private CombatStateController m_CombatStateController = null;
		[SerializeField]private bool IsRandom = true;

		[SerializeField]private string filename = "Net1";

		[SerializeField]private NeuralNet m_Enemy = null;

		private float[,] moveNet = { {1f, 3f, -3.5f, -0.75f, -3f, 3.5f, 0.75f, -3f},
									{0, 0, 0, 0, 0, 0, 0, 0},
									{0, 0, 0, 0, 0, 0, 0, 0}	
									};
		private float[,] jumpNet = { {0, -1f, -3.5f, 0, 2f, 3.5f, 0, -2f},
									{0, 0, 0, 0, 0, 0, 0, 0},
									{0, -1, 2, 0, 2, 2, 0, -2}	
									};
		private float[,] attackNet = { {0, -1f, -3.5f, 0, 2f, 3.5f, 0, -2f},
									{0, 0, 0, 0, 0, 0, 0, 0},
									{0, 0, 0, 0, 0, 0, 0, 0}		
									};

		public void InitNet()
		{
			m_CombatStateController.OnRoundOver += UpdateWeights;

			if(!IsRandom)
			{
				FileInfo fileInf = new FileInfo ($"{filename}.dat");
				if (fileInf.Exists)
				{
					try
					{ 
						int i = 0;
						using (BinaryReader reader = new BinaryReader(File.Open($"{filename}.dat", FileMode.Open)))
						{
							while (reader.PeekChar() > -1)
							{
								float value = (float)reader.ReadDouble();
								if(i < 24)
								{
									moveNet[i / 8, i % 8] = value;
								} else if(i < 48)
								{
									jumpNet[i / 8 - 3, i % 8] = value;
								}
								else
								{
									attackNet[i / 8 - 6, i % 8] = value;
								}
								i++;
							}
						}
					}
					catch (Exception e)
					{
						Debug.Log(e.Message);
					}
				}
			}
			else
			{
				for(int i = 0; i < 3; i++)
					for(int j = 0; j < 8; j++)
					{
						moveNet[i, j] = UnityEngine.Random.Range(-5, 5);
						jumpNet[i, j] = UnityEngine.Random.Range(-5, 5);
						attackNet[i, j] = UnityEngine.Random.Range(-5, 5);
					}
			}
		}

		public void CombatAnalysis(out float vertical, out float horizontal, out bool IsAttack, float distance, int playerState, int aiState)
		{
			vertical = JumpResult(distance, playerState, aiState);
			horizontal = MoveResult(distance, playerState, aiState);
			IsAttack = AttackResult(distance, playerState, aiState);
		}

		float JumpResult(float distance, int playerState, int aiState)
		{
			float result = 0f;
			result += jumpNet[0, 0] * distance + jumpNet[0, 1];
			result += jumpNet[1, 0] * playerState + jumpNet[1, 1];
			result += jumpNet[2, 0] * aiState + jumpNet[2, 1];
			for(int i = 1; i < 3; i++)
			{
				result += (jumpNet[0, 3*i] * distance + jumpNet[0, 3*i + 1]) * Heviside(distance - jumpNet[0, 3*i - 1]);
				result += (jumpNet[1, 3*i] * playerState + jumpNet[1, 3*i + 1]) * Heviside(playerState - jumpNet[1, 3*i - 1]);
				result += (jumpNet[2, 3*i] * aiState + jumpNet[2, 3*i + 1]) * Heviside(aiState - jumpNet[2, 3*i - 1]);
			}
			if(result <= 0f)
				return 0f;
			return 1f;
		}

		float MoveResult(float distance, int playerState, int aiState)
		{
			float result = 0f;
			result += moveNet[0, 0] * distance + moveNet[0, 1];
			result += moveNet[1, 0] * playerState + moveNet[1, 1];
			result += moveNet[2, 0] * aiState + moveNet[2, 1];
			for(int i = 1; i < 3; i++)
			{
				result += (moveNet[0, 3*i] * distance + moveNet[0, 3*i + 1]) * Heviside(distance - moveNet[0, 3*i - 1]);
				result += (moveNet[1, 3*i] * playerState + moveNet[1, 3*i + 1]) * Heviside(playerState - moveNet[1, 3*i - 1]);
				result += (moveNet[2, 3*i] * aiState + moveNet[2, 3*i + 1]) * Heviside(aiState - moveNet[2, 3*i - 1]);
			}
			if(result < -1f)
				return -1f;
			if(result >= -1f && result < 1f)
				return 0f;
			return 1f;
		}

		bool AttackResult(float distance, int playerState, int aiState)
		{
			float result = 0f;
			result += attackNet[0, 0] * distance + attackNet[0, 1];
			result += attackNet[1, 0] * playerState + attackNet[1, 1];
			result += attackNet[2, 0] * aiState + attackNet[2, 1];
			for(int i = 1; i < 3; i++)
			{
				result += (attackNet[0, 3*i] * distance + attackNet[0, 3*i + 1]) * Heviside(distance - attackNet[0, 3*i - 1]);
				result += (attackNet[1, 3*i] * playerState + attackNet[1, 3*i + 1]) * Heviside(playerState - attackNet[1, 3*i - 1]);
				result += (attackNet[2, 3*i] * aiState + attackNet[2, 3*i + 1]) * Heviside(aiState - attackNet[2, 3*i - 1]);
			}
			return result >= 0f;
		}

		void UpdateWeights(int winner, int currentRound, int result) // 0 - draw; -1 - 1; 1 - 2
		{
			if(result != 0 && numberOfPlayer != result || result == 0)
			{
				Adaptation();
			}
		}

		void Adaptation()
		{
			for(int i = 0; i < 3; i++)
				for(int j = 0; j < 2; j++)
				{
					moveNet[i, j] = (moveNet[i, j] + m_Enemy.moveNet[i, j]) / 2 + UnityEngine.Random.Range(-0.2f, 0.2f);
					jumpNet[i, j] = (jumpNet[i, j] + m_Enemy.jumpNet[i, j]) / 2 + UnityEngine.Random.Range(-0.2f, 0.2f);
					attackNet[i, j] = (attackNet[i, j] + m_Enemy.attackNet[i, j]) / 2 + UnityEngine.Random.Range(-0.2f, 0.2f);
				}
		}

		void OnDestroy()
		{
			try
			{
				using (BinaryWriter writer = new BinaryWriter(File.Open($"{filename}.dat", FileMode.OpenOrCreate)))
				{
					foreach (var s in moveNet)
					{
						writer.Write(s);
					}
					foreach (var s in jumpNet)
					{
						writer.Write(s);
					}
					foreach (var s in attackNet)
					{
						writer.Write(s);
					}
				}
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}
		}

		float Heviside(float x)
		{
			if(x >= 0)
				return 1;
			return 0;
		}
	}
}
