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
	}

	void SaveVideoSettings()
	{
		XmlDocument xDoc = new XmlDocument();
		XmlNode xRoot = xDoc.CreateElement ("Video");
		xDoc.AppendChild (xRoot);
		
		SaveData(ref xDoc, ref xRoot, "Resolution", new Dictionary<string, object> { 
			{"Width", GameVideo.ScreenResolution.width},
			{"Height", GameVideo.ScreenResolution.height}
			});
			
		xDoc.Save(videoSettingsFilePath);
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

	void SaveInputSettings()
	{
		GameInput.SaveSettings(inputSettingsFilePath);
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
		xDoc.Load(audioSettingsFilePath);
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
		xDoc.Load(videoSettingsFilePath);
		XmlElement xRoot = xDoc.DocumentElement;
		foreach(XmlNode xData in xRoot)
		{
			switch(xData.Name)
			{
				case "Resolution":
					int width = int.Parse(xData.Attributes.GetNamedItem("Width").Value);
					int height = int.Parse(xData.Attributes.GetNamedItem("Height").Value);
					Screen.SetResolution(width, height, true);
					GameVideo.ScreenResolution = Screen.currentResolution;
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
