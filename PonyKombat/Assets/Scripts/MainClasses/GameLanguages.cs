using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public static class GameLanguages
{
	private enum Languages
	{ English, Russian, Belarusian }

	private static Dictionary<string, List<string>> Localizations = new Dictionary<string, List<string>>{};
	private static Languages currentLanguage = Languages.English;
	public static event Action OnLanguageChanged;

	public static string GetCurrentLocalization(string key)
	{
		int current = (int)currentLanguage;
		return Localizations[key][current];
	}

	public static string GetCurrentLanguage()
	{
		return currentLanguage.ToString();
	}

	public static void ChangeCurrentLanguage(string newLanguage)
	{
		switch(newLanguage)
		{
			case "English":
				currentLanguage = Languages.English;
				break;
			case "Russian":
				currentLanguage = Languages.Russian;
				break;
			case "Belarusian":
				currentLanguage = Languages.Belarusian;
			break;
			default:
				throw new NotImplementedException($"{newLanguage} not implemented yet");
		}
		OnLanguageChanged?.Invoke();
	}

	private static string filePath = @"Localization\Localization.xml";
	public static void SaveCurrentLanguage()
	{
		XmlDocument xDoc = new XmlDocument();
		xDoc.Load(filePath);
		XmlElement xRoot = xDoc.DocumentElement;
		xRoot.ChildNodes[0].Attributes.GetNamedItem("Value").Value = currentLanguage.ToString();
		xDoc.Save(filePath);
	}

	public static void LoadLocalization()
	{
		FileInfo fileInf = new FileInfo (filePath);
		if (!fileInf.Exists)
		{
			File.WriteAllText("ErrorLog", "Can't find localization file");
			UnityEngine.Application.Quit();
			return;
		}
		XmlDocument xDoc = new XmlDocument();
		xDoc.Load(filePath);
		XmlElement xRoot = xDoc.DocumentElement;
		string language = xRoot.ChildNodes[0].Attributes.GetNamedItem("Value").Value;
		for(int i = 1; i < xRoot.ChildNodes.Count; i++)
		{
			List<string> values = new List<string>{};
			foreach(XmlNode cur in xRoot.ChildNodes[i].Attributes)
				values.Add(cur.Value);
			Localizations.Add(xRoot.ChildNodes[i].Name, values);
		}

		ChangeCurrentLanguage(language);
	}
}
