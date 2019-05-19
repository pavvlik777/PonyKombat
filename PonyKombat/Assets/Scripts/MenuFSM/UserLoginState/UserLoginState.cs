using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

namespace n_MenuFSM
{
	public class UserLoginState : State
	{
		[SerializeField]private MenuFSM menu = null;
		[SerializeField]private InputField m_Login = null;
		[SerializeField]private InputField m_Password = null;

		[SerializeField]private GameObject incorrectLoginMessageBox = null;
		[SerializeField]private GameObject incorrectPasswordMessageBox = null;
		[SerializeField]private GameObject registerMessageBox = null;
		private GameObject messageBox = null;

		private string profilesSettingsFolder = @"Profiles\";

		void Awake()
		{
			Directory.CreateDirectory("Profiles");
            m_Login.text = "";
            m_Password.text = "";
		}

		void Start()
		{
			if(GameUser.isLogined)
				menu.ChangeState(StatesNames.MainMenu);
		}

		public void TryToRegister()
		{
			string login = m_Login.text;
			string password = m_Password.text;
			if(string.IsNullOrWhiteSpace(login))
			{
				ShowMessageBox(incorrectLoginMessageBox);
				return;
			}
			if(string.IsNullOrWhiteSpace(password))
			{
				ShowMessageBox(incorrectPasswordMessageBox);
				return;
			}
			FileInfo fileInf = new FileInfo ($"{profilesSettingsFolder}{login}.xml");
			if (fileInf.Exists)
			{
				ShowMessageBox(registerMessageBox);
				return;
			}
			RegisterSetUserInfo(login, password);
			GameUser.isLogined = true;
			menu.ChangeState(StatesNames.MainMenu);
		}

		void RegisterSetUserInfo(string login, string password)
		{
			GameUser.login = login;
			GameUser.password = password;
			GameUser.wins = 0;
			GameUser.draws = 0;
			GameUser.loses = 0;
			GameUser.achievementStatus = new bool[4];
			for(int i = 0 ; i < 4; i++)
				GameUser.achievementStatus[i] = false;
		}

		public void TryToLogin()
		{
			string login = m_Login.text;
			FileInfo fileInf = new FileInfo ($"{profilesSettingsFolder}{login}.xml");
			if (!fileInf.Exists)
			{
				ShowMessageBox(incorrectLoginMessageBox);
				return;
			}
			XmlDocument xDoc = new XmlDocument();
			AES.DecryptFile($"{profilesSettingsFolder}{login}.xml", true);
			xDoc.Load($"{profilesSettingsFolder}{login}.xml");
			AES.EncryptFile($"{profilesSettingsFolder}{login}.xml", true);
			XmlElement xRoot = xDoc.DocumentElement;
			string password = m_Password.text;
			foreach(XmlNode xNode in xRoot)
			{
				switch(xNode.Name)
				{
					case "Password":
						if(password != xNode.Attributes.GetNamedItem("Value").Value)
						{
							ShowMessageBox(incorrectPasswordMessageBox);
							return;
						}
					break;
					case "Wins":
						GameUser.wins = int.Parse(xNode.Attributes.GetNamedItem("Value").Value);
					break;
					case "Draws":
						GameUser.draws = int.Parse(xNode.Attributes.GetNamedItem("Value").Value);
					break;
					case "Loses":
						GameUser.loses = int.Parse(xNode.Attributes.GetNamedItem("Value").Value);
					break;
					case "AchievementsAmount":
						int amount = int.Parse(xNode.Attributes.GetNamedItem("Value").Value);
						GameUser.achievementStatus = new bool[amount];
					break;
					case "Achievement":
						int id = int.Parse(xNode.Attributes.GetNamedItem("ID").Value);
						bool status = bool.Parse(xNode.Attributes.GetNamedItem("Status").Value);
						GameUser.achievementStatus[id] = status;
					break;
				}
			}
			GameUser.login = login;
			GameUser.password = password;
			GameUser.isLogined = true;
			menu.ChangeState(StatesNames.MainMenu);
		}

		void SaveData(ref XmlDocument xDoc, ref XmlNode xRoot, string name, Dictionary<string, object> values)
		{
			XmlNode dataNode = xDoc.CreateElement(name);
			foreach(var cur in values)
			{
				XmlAttribute newAttr = xDoc.CreateAttribute(cur.Key);
				newAttr.Value = cur.Value.ToString();
				dataNode.Attributes.Append(newAttr);
			}
			xRoot.AppendChild(dataNode);
		}

		void ShowMessageBox(GameObject _messageBox)
		{
			messageBox = _messageBox;
			messageBox.SetActive(true);
			menu.LockChangeState();
		}

		public void HideMessageBox()
		{
			messageBox.SetActive(false);
			menu.UnlockChangeState();
		}
	}
}
