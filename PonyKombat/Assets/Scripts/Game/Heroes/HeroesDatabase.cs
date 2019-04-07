using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace n_Game.Combat
{
	public class HeroesDatabase : MonoBehaviour //Singleton
	{
		private List<Hero> Heroes = null;
		[Header("Player controllers")]
		[SerializeField]private List<HeroController> playerControllers = null;
		[Header("AI controllers")]
		[SerializeField]private List<HeroController> AIControllers = null;

		public HeroController this[HeroesNames s, bool IsPlayer]
		{
			get
			{
				List<HeroController> temp = IsPlayer ? playerControllers : AIControllers;
				if(s == HeroesNames.Random)
				{
					int n = UnityEngine.Random.Range(0, temp.Count);
					//temp[n].gameObject.SetActive(true);
					return temp[n];
				}
				foreach(var cur in temp)
					if(cur.HeroName == s)
					{
						//cur.gameObject.SetActive(true);
						return cur;
					}
				throw new ArgumentException("There are no hero with such name");
			}
		}

		public Hero this[int i]
		{
			get { return Heroes[i]; }
		}

		public Hero this[HeroesNames s]
		{
			get
			{
				if(s == HeroesNames.Random)
				{
					int n = UnityEngine.Random.Range(0, Heroes.Count);
					return Heroes[n];
				}
				foreach(var cur in Heroes)
					if(cur.heroName == s)
						return cur;
				throw new ArgumentException("There are no hero with such name");
			}
		}

		public static HeroesDatabase instance = null;
		void Awake()
		{
			if(instance && instance != this)
			{
				Debug.LogError("Another instance of heroes database");
				Destroy(this);
				return;
			}
			instance = this;
			LoadHeroesData();

			foreach(var cur in playerControllers)
				cur.gameObject.SetActive(false);
			foreach(var cur in AIControllers)
				cur.gameObject.SetActive(false);
		}
		
		private string filePath = "Heroes";
		private Dictionary<string, Hero> heroesNames = new Dictionary<string, Hero>
		{
			{@"\Applejack.xml", new Hero(3f, 100f, new Dictionary<string, float> { {"X", 5f} }, HeroesNames.Applejack)},
			{@"\Fluttershy.xml", new Hero(3f, 100f, new Dictionary<string, float> { {"X", 5f} }, HeroesNames.Fluttershy)},
			{@"\PinkiePie.xml", new Hero(3f, 100f, new Dictionary<string, float> { {"X", 5f} }, HeroesNames.PinkiePie)}
		};
		void LoadHeroesData()
		{
			Directory.CreateDirectory(filePath);
			Heroes = new List<Hero> {};
			foreach(var cur in heroesNames)
			{
				string fullFilepath = $"{filePath}{cur.Key}";
				FileInfo fileInf = new FileInfo (fullFilepath);
				if (!fileInf.Exists)
				{
					Heroes.Add(cur.Value);
					SaveHeroData(fullFilepath, cur.Value);
					continue;
				}
				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(fullFilepath);
				XmlElement xRoot = xDoc.DocumentElement;
				Hero output = new Hero();
				foreach(XmlNode xNode in xRoot)
				{
					switch(xNode.Name)
					{
						case "MoveSpeed":
							output.moveSpeed = float.Parse(xNode.Attributes.GetNamedItem("Value").Value);
						break;
						case "MaxHP":
							output.maxHP = float.Parse(xNode.Attributes.GetNamedItem("Value").Value);
						break;
						case "Combos":
							int amount = int.Parse(xNode.Attributes.GetNamedItem("Amount").Value);
							for(int i = 0; i < amount; i++)
							{
								XmlNode xComboNode = xNode.ChildNodes[i];
								string _buttons = xComboNode.Attributes.GetNamedItem("Buttons").Value;
								float _damage = float.Parse(xComboNode.Attributes.GetNamedItem("Damage").Value);
								output.combos.Add(_buttons, _damage);
							}
						break;
						case "HeroName":
							output.heroName = (HeroesNames)Enum.Parse(typeof(HeroesNames), xNode.Attributes.GetNamedItem("Value").Value);
						break;
						default:
							throw new ArgumentException("Wrong data in file");
					}
				}
				Heroes.Add(output);
			}
		}

		void SaveHeroData(string filePath, Hero hero)
		{
			XmlDocument xDoc = new XmlDocument();
			XmlNode xRoot = xDoc.CreateElement ("Hero");
			xDoc.AppendChild (xRoot);
			
			SaveData(ref xDoc, ref xRoot, "MoveSpeed", new Dictionary<string, object>{
				{"Value", hero.moveSpeed}
			});
			SaveData(ref xDoc, ref xRoot, "MaxHP", new Dictionary<string, object>{
				{"Value", hero.maxHP}
			});
			XmlNode xCombosNode = SaveData(ref xDoc, ref xRoot, "Combos", new Dictionary<string, object>{
				{"Amount", hero.combos.Count}
			});
			foreach(var cur in hero.combos)
			{
				SaveData(ref xDoc, ref xCombosNode, "Combo", new Dictionary<string, object>{
				{"Damage", cur.Value},
				{"Buttons", cur.Key}
				});
			}
			SaveData(ref xDoc, ref xRoot, "HeroName", new Dictionary<string, object>{
				{"Value", hero.heroName}
			});

			xDoc.Save(filePath);
		}

		XmlNode SaveData(ref XmlDocument xDoc, ref XmlNode xRoot, string name, Dictionary<string, object> values)
		{
			XmlNode dataNode = xDoc.CreateElement(name);
			foreach(var cur in values)
			{
				XmlAttribute newAttr = xDoc.CreateAttribute(cur.Key);
				newAttr.Value = cur.Value.ToString();
				dataNode.Attributes.Append(newAttr);
			}
			xRoot.AppendChild(dataNode);
			return dataNode;
		}
	}
}
