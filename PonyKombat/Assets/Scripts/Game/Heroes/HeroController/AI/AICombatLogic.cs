using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

namespace n_Game.Combat
{
	public class AICombatLogic : MonoBehaviour
	{
		[Header("Player data")]
		[SerializeField]private HeroController m_PlayerController = null;
		[SerializeField]private Transform m_PlayerTransform = null;
		
		[Header("AI data")]
		[SerializeField]private HeroController m_AIController = null;
		[SerializeField]private Transform m_AITransform = null;

		[Header("Behaviour data")]
		[SerializeField]private float m_SafeDistance = 3.5f;
		[SerializeField]private float m_DodgeDelay = 0.3f;
		[SerializeField]private float m_AttackDelay = 0.2f;
		
		[SerializeField]private bool IsAFK = false;

		private Control.StatesNames previousPlayerState = Control.StatesNames.Walk;
		private bool IsNeedToDodgeAttack = false;
		private bool IsNeedToAttack = false;

		private bool IsTakingDecision = false;

		private string settingsFilePath = @"Settings\AI.xml";
		public void LoadAISettings()
		{
			FileInfo fileInf = new FileInfo (settingsFilePath);
			if (!fileInf.Exists)
			{
				return;
			}
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(settingsFilePath);
			XmlElement xRoot = xDoc.DocumentElement;
			foreach(XmlNode xNode in xRoot)
			{
				switch(xNode.Name)
				{
					case "DodgeDelay":
						m_DodgeDelay = float.Parse(xNode.Attributes.GetNamedItem("Value").Value);
					break;
					case "AttackDelay":
						m_AttackDelay = float.Parse(xNode.Attributes.GetNamedItem("Value").Value);
					break;
				}
			}
		}

		public void SetEnemyData(HeroController enemyController)
		{
			m_PlayerController = enemyController;
			m_PlayerTransform = enemyController.transform;
		}

		public void CombatAnalysis(out float vertical, out float horizontal, out bool IsAttack, Control.AICombosRegistration m_CombosRegistrarion)
		{ //TBD delays not work properly
			vertical = 0f;
			horizontal = 0f;
			IsAttack = false;
			if(IsAFK)
				return;
			float currentDistance = GetDistance();
			if(Math.Abs(currentDistance) < m_SafeDistance)
			{
				horizontal = 0f;
				if(m_PlayerController.CurrentFSMState == Control.StatesNames.Attack && previousPlayerState != Control.StatesNames.Attack)
				{
					StopAllCoroutines();
					StartCoroutine(DodgeDelay());
				}
				else if(m_PlayerController.CurrentFSMState != Control.StatesNames.Attack && !IsTakingDecision)
				{
					IsTakingDecision = true;
					StopAllCoroutines();
					StartCoroutine(AttackDelay());
				}
				
				if(IsNeedToDodgeAttack)
				{
					vertical = 1f;
					IsAttack = false;
					IsNeedToDodgeAttack = false;
					IsNeedToAttack = false;
				}
				else if(IsNeedToAttack)
				{
					vertical = 0f;
					IsAttack = true;
					int numberOfCombo = UnityEngine.Random.Range(0,4); //TBD
					m_CombosRegistrarion.ChooseCombo(numberOfCombo);
					IsNeedToDodgeAttack = false;
					IsNeedToAttack = false;
				}
				else
				{
					vertical = 0f;
					IsAttack = false;
				}
			}
			else if(currentDistance < 0f)
			{
				IsNeedToAttack = false;
				IsNeedToDodgeAttack = false;
				IsTakingDecision = false;
				vertical = 0f;
				horizontal = -1f;
				IsAttack = false;
			}
			else
			{
				IsNeedToAttack = false;
				IsNeedToDodgeAttack = false;
				IsTakingDecision = false;
				vertical = 0f;
				horizontal = 1f;
				IsAttack = false;
			}
			previousPlayerState= m_PlayerController.CurrentFSMState;
		}

		IEnumerator DodgeDelay()
		{
			float timePassed = 0f;
			while(timePassed <= m_DodgeDelay)
			{
				timePassed += GameTime.deltaTime;
				yield return null;
			}
			IsNeedToDodgeAttack = true;
			IsTakingDecision = false;
			yield break;
		}

		IEnumerator AttackDelay()
		{
			float timePassed = 0f;
			while(timePassed <= m_AttackDelay)
			{
				timePassed += GameTime.deltaTime;
				yield return null;
			}
			IsNeedToAttack = true;
			IsTakingDecision = false;
			yield break;
		}

		float GetDistance()
		{
			return m_PlayerTransform.localPosition.x - m_AITransform.localPosition.x;
		}
	}
}
