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
	string inputSettingsFilePath = @"Settings\Input.xml";
	
	void Awake()
	{
		if(!IsSettingsLoaded)
		{
			LoadAudioSettings();
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
		SaveInputSettings();
	}

	#region SaveMethods
	void SaveAudioSettings()
	{
		XmlDocument xDoc = new XmlDocument();
		XmlNode xRoot = xDoc.CreateElement ("Volumes");
		xDoc.AppendChild (xRoot);
		
		SaveVolume(ref xDoc, ref xRoot, "MenuMusic", GameSounds.MenuMusicVolume);
		SaveVolume(ref xDoc, ref xRoot, "MenuSounds", GameSounds.MenuSoundsVolume);
		SaveVolume(ref xDoc, ref xRoot, "GameMusic", GameSounds.GameMusicVolume);
		SaveVolume(ref xDoc, ref xRoot, "GameSounds", GameSounds.GameSoundsVolume);
			
		xDoc.Save(audioSettingsFilePath);
	}

	void SaveVolume(ref XmlDocument xDoc, ref XmlNode xRoot, string name, float value)
	{
		XmlNode volumeNode = xDoc.CreateElement(name);
		XmlAttribute xAttr = xDoc.CreateAttribute("Value");
		xAttr.Value = value.ToString();
		volumeNode.Attributes.Append(xAttr);
		xRoot.AppendChild (volumeNode);
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

	void LoadInputSettings()
	{
		GameInput.LoadSettings(inputSettingsFilePath);
	}
	#endregion
}
