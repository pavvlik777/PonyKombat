using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public sealed class AppMain : MonoBehaviour
{
	private static bool IsSettingsLoaded = false;

	private AppMain()
	{  }

	string audioSettingsFilePath = @"Settings\Audio.xml";
	string videoSettingsFilePath = @"Settings\Video.xml";
	string inputSettingsFilePath = @"Settings\Input.xml";
	
	void Awake()
	{
		Directory.CreateDirectory("Settings");
		if(!IsSettingsLoaded)
		{
			GameLanguages.LoadLocalization();
			LoadAudioSettings();
			LoadVideoSettings();
			LoadInputSettings();
			IsSettingsLoaded = true;
		}
	}

	void Update()
	{
		GameInput.UpdateAxesValues();
	}

	void OnDestroy()
	{
		OnApplicationQuit();
	}

	void OnApplicationQuit()
	{
		GameLanguages.SaveCurrentLanguage();
		SaveAudioSettings();
		SaveVideoSettings();
		SaveInputSettings();
	}

	#region SaveMethods
	void SaveAudioSettings()
	{
		XmlDocument xDoc = new XmlDocument();
		XmlNode xRoot = xDoc.CreateElement ("Volumes");
		xDoc.AppendChild (xRoot);
		
		SaveData(ref xDoc, ref xRoot, "MenuMusic", new Dictionary<string, object> { 
			{"Value", GameSounds.MenuMusicVolume} 
			});
		SaveData(ref xDoc, ref xRoot, "MenuSounds", new Dictionary<string, object> { 
			{"Value", GameSounds.MenuSoundsVolume} 
			});
		SaveData(ref xDoc, ref xRoot, "GameMusic", new Dictionary<string, object> { 
			{"Value", GameSounds.GameMusicVolume} 
			});
		SaveData(ref xDoc, ref xRoot, "GameSounds", new Dictionary<string, object> { 
			{"Value", GameSounds.GameSoundsVolume} 
			});
			
		xDoc.Save(audioSettingsFilePath);
		AES.EncryptFile(audioSettingsFilePath, true);
	}

	void SaveVideoSettings()
	{
		XmlDocument xDoc = new XmlDocument();
		XmlNode xRoot = xDoc.CreateElement ("Video");
		xDoc.AppendChild (xRoot);
		
		SaveData(ref xDoc, ref xRoot, "Resolution", new Dictionary<string, object> { 
			{"Width", GameVideo.screenResolution.width},
			{"Height", GameVideo.screenResolution.height}
			});
		SaveData(ref xDoc, ref xRoot, "Anisotropic", new Dictionary<string, object> { 
			{"Value", GameVideo.anisotropicFiltering}
			});
		SaveData(ref xDoc, ref xRoot, "AntiAliasing", new Dictionary<string, object> { 
			{"Value", GameVideo.antiAliasing}
			});
			
		xDoc.Save(videoSettingsFilePath);
		AES.EncryptFile(videoSettingsFilePath, true);
	}

	void SaveInputSettings()
	{
		GameInput.SaveSettings(inputSettingsFilePath);
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
	#endregion

	#region LoadMethods
	void LoadAudioSettings()
	{
		FileInfo fileInf = new FileInfo (audioSettingsFilePath);
		if (!fileInf.Exists)
		{
			GameSounds.SetDefaultVolume();
			return;
		}
		XmlDocument xDoc = new XmlDocument();
		AES.DecryptFile(audioSettingsFilePath, true);
		xDoc.Load(audioSettingsFilePath);
		AES.EncryptFile(audioSettingsFilePath, true);
		XmlElement xRoot = xDoc.DocumentElement;
		foreach(XmlNode xVolume in xRoot)
		{
			switch(xVolume.Name)
			{
				case "MenuMusic":
					GameSounds.MenuMusicVolume = float.Parse(xVolume.Attributes.GetNamedItem("Value").Value);
				break;
				case "MenuSounds":
					GameSounds.MenuSoundsVolume = float.Parse(xVolume.Attributes.GetNamedItem("Value").Value);
				break;
				case "GameMusic":
					GameSounds.GameMusicVolume = float.Parse(xVolume.Attributes.GetNamedItem("Value").Value);
				break;
				case "GameSounds":
					GameSounds.GameSoundsVolume = float.Parse(xVolume.Attributes.GetNamedItem("Value").Value);
				break;
				default:
					throw new ArgumentException("Wrong data in file");
			}
		}
	}

	void LoadVideoSettings()
	{
		FileInfo fileInf = new FileInfo (videoSettingsFilePath);
		if (!fileInf.Exists)
		{
			GameVideo.SetDefaultSettings();
			return;
		}
		XmlDocument xDoc = new XmlDocument();
		AES.DecryptFile(videoSettingsFilePath, true);
		xDoc.Load(videoSettingsFilePath);
		AES.EncryptFile(videoSettingsFilePath, true);
		XmlElement xRoot = xDoc.DocumentElement;
		foreach(XmlNode xData in xRoot)
		{
			switch(xData.Name)
			{
				case "Resolution":
					int width = int.Parse(xData.Attributes.GetNamedItem("Width").Value);
					int height = int.Parse(xData.Attributes.GetNamedItem("Height").Value);
					Screen.SetResolution(width, height, true);
					GameVideo.screenResolution = Screen.currentResolution;
				break;
				case "Anisotropic":
					bool filtering = bool.Parse(xData.Attributes.GetNamedItem("Value").Value);
					GameVideo.SetAnisotropicFiltering(filtering);
				break;
				case "AntiAliasing":
					int aliasing = int.Parse(xData.Attributes.GetNamedItem("Value").Value);
					GameVideo.SetAntiAliasing(aliasing);
				break;
				default:
					throw new ArgumentException("Wrong data in file");
			}
		}
	}

	void LoadInputSettings()
	{
		GameInput.LoadSettings(inputSettingsFilePath);
	}
	#endregion
}
