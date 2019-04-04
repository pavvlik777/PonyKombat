using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public static class GameInput
{
	#region InputButton
	[Serializable]
	private class InputButton
	{ 
		public string Name;
		public string Description;

		public KeyCode MainKey;
		public KeyCode AltKey;

		public InputButton() : this("", "", KeyCode.None, KeyCode.None)
		{}

		public InputButton(string name, string descr, KeyCode main, KeyCode alt)
		{
			Name = name;
			Description = descr;
			MainKey = main;
			AltKey = alt;
		}

		public InputButton(InputButton other)
		{
			Name = other.Name;
			Description = other.Description;
			MainKey = other.MainKey;
			AltKey = other.AltKey;
		}

		public bool IsPressed
		{ get { return Input.GetKey (MainKey) || Input.GetKey (AltKey); } }

		public bool IsDown
		{ get { return Input.GetKeyDown (MainKey) || Input.GetKeyDown (AltKey); } }

		public bool IsUp
		{ get { return Input.GetKeyUp (MainKey) || Input.GetKeyUp (AltKey); } }

		public void Serialize(ref XmlNode axeNode, ref XmlDocument xDoc)
		{
			XmlNode buttonNode = xDoc.CreateElement ("Button");
			XmlAttribute nameAttribute = xDoc.CreateAttribute ("name");
			nameAttribute.Value = this.Name;
			buttonNode.Attributes.Append (nameAttribute);
			XmlAttribute descrAttribute = xDoc.CreateAttribute ("description");
			descrAttribute.Value = this.Description;
			buttonNode.Attributes.Append (descrAttribute);

			XmlAttribute mainButtonAttribute = xDoc.CreateAttribute ("mainKey");
			mainButtonAttribute.Value = this.MainKey.ToString ();
			buttonNode.Attributes.Append (mainButtonAttribute);
			XmlAttribute altButtonAttribute = xDoc.CreateAttribute ("altKey");
			altButtonAttribute.Value = this.AltKey.ToString ();
			buttonNode.Attributes.Append (altButtonAttribute);

			axeNode.AppendChild (buttonNode);
		}

		public static InputButton Deserialize(XmlNode xButton)
		{
			string bName = xButton.Attributes.GetNamedItem ("name").Value;
			string description = xButton.Attributes.GetNamedItem("description").Value;
			KeyCode bMainButton = (KeyCode)Enum.Parse(typeof(KeyCode), xButton.Attributes.GetNamedItem ("mainKey").Value);
			KeyCode bAltButton = (KeyCode)Enum.Parse(typeof(KeyCode), xButton.Attributes.GetNamedItem ("altKey").Value);
			return new InputButton (bName, description, bMainButton, bAltButton);
		}
	}
	#endregion

	#region InputAxe
	[Serializable]
	private class InputAxe
	{
		public string Name;

		public InputButton Positive;
		public InputButton Negative;

		public float Gravity;
		public float Sensivity;
		public float Dead;

		private float value;
		public float Value
		{ get {return value; } }

		void ResetValue()
		{ value = 0f; }

		public void ChangeValue()
		{
			if (Positive.IsPressed) {
				if (value < -Dead) {
					value = Sensivity * Time.deltaTime;
				} else {
					value += Sensivity * Time.deltaTime;
				}
				if (value > 1f)
					value = 1f;
			} else if (Negative.IsPressed) {
				if (value > Dead) {
					value = -Sensivity * Time.deltaTime;
				} else {
					value -= Sensivity * Time.deltaTime;
				}
				if (value < -1f)
					value = -1f;
			} else {
				if (value < Dead && value > -Dead) {
					ResetValue ();
				} else if (value > Dead) {
					value -= Gravity * Time.deltaTime;
					if (value < 0f)
						value = 0f;
				} else {
					value += Gravity * Time.deltaTime;
					if (value > 0f)
						value = 0f;
				}
			}
		}

		public InputAxe () : this ("", 0f, 0f, 0f, new InputButton (), new InputButton ())
		{ }

		public InputAxe(string name, float gravity, float sensivity, float dead, InputButton positive, InputButton negative)
		{
			Name = name;
			Gravity = gravity;
			Sensivity = sensivity;
			Dead = dead;
			Positive = positive;
			Negative = negative;
		}

		public InputAxe(InputAxe other)
		{
			Name = other.Name;

			Positive = new InputButton (other.Positive);
			Negative = new InputButton (other.Negative);

			Gravity = other.Gravity;
			Sensivity = other.Sensivity;
			Dead = other.Dead;

			value = 0f;
		}

		public void Serialize(ref XmlNode axeNode, ref XmlDocument xDoc)
		{
			XmlAttribute nameAttribute = xDoc.CreateAttribute ("name");
			nameAttribute.Value = this.Name;
			axeNode.Attributes.Append (nameAttribute);

			XmlAttribute gravityAttribute = xDoc.CreateAttribute ("gravity");
			gravityAttribute.Value = this.Gravity.ToString ();
			axeNode.Attributes.Append (gravityAttribute);
			XmlAttribute sensivityAttribute = xDoc.CreateAttribute ("sensivity");
			sensivityAttribute.Value = this.Sensivity.ToString ();
			axeNode.Attributes.Append (sensivityAttribute);
			XmlAttribute deadAttribute = xDoc.CreateAttribute ("dead");
			deadAttribute.Value = this.Dead.ToString();
			axeNode.Attributes.Append (deadAttribute);

			Positive.Serialize (ref axeNode, ref xDoc);
			Negative.Serialize (ref axeNode, ref xDoc);
		}

		public static InputAxe Deserialize(XmlNode xAxe)
		{
			string aName = xAxe.Attributes.GetNamedItem ("name").Value;
			float aGravity = float.Parse(xAxe.Attributes.GetNamedItem ("gravity").Value);
			float aSensivity = float.Parse(xAxe.Attributes.GetNamedItem ("sensivity").Value);
			float aDead = float.Parse(xAxe.Attributes.GetNamedItem ("dead").Value);

			InputButton aPositive = new InputButton ();
			InputButton aNegative = new InputButton ();
			bool isPositive = true;
			foreach (XmlNode xButton in xAxe) {
				if (xButton.Name == "Button") {
					if (isPositive) {
						isPositive = false;
						aPositive = InputButton.Deserialize (xButton);
					} else {
						aNegative = InputButton.Deserialize (xButton);
					}
				}
			}
			return new InputAxe (aName, aGravity, aSensivity, aDead, aPositive, aNegative);
		}
	}
	#endregion

	static List<InputAxe> Axes;
	static List<InputButton> Buttons;

	public static void UpdateAxesValues()
	{
		foreach(var cur in Axes)
			cur.ChangeValue ();
	}

	public static List<(string, string)> GetButtonsNames()
	{
		List<(string, string)> output = new List<(string, string)>(){};
		foreach(var cur in Buttons)
			output.Add((cur.Name, cur.Description));
		return output;
	}

	static void LoadInitSettings()
	{
		Buttons = new List<InputButton> ()
		{
			new InputButton("Up", "UpButton", KeyCode.UpArrow, KeyCode.None),
			new InputButton("Down", "DownButton", KeyCode.DownArrow, KeyCode.None),
			new InputButton("Right", "RightButton",  KeyCode.RightArrow, KeyCode.None),
			new InputButton("Left", "LeftButton",  KeyCode.LeftArrow, KeyCode.None),
			
			new InputButton("X", "AttackButtonX",  KeyCode.Q, KeyCode.None),
			new InputButton("Y", "AttackButtonY", KeyCode.W, KeyCode.None),
			new InputButton("A", "AttackButtonA", KeyCode.E, KeyCode.None),
			new InputButton("B", "AttackButtonB", KeyCode.R, KeyCode.None)
		};
		Axes = new List<InputAxe>()
		{
			new InputAxe("Vertical", 1000f, 1000f, 0.001f, new InputButton(Buttons[0]), new InputButton(Buttons[1])),
			new InputAxe("Horizontal", 3f, 3f, 0.001f, new InputButton(Buttons[2]), new InputButton(Buttons[3])),
			new InputAxe("AttackX", 1000f, 1000f, 0.001f, new InputButton(Buttons[4]), new InputButton()),
			new InputAxe("AttackY", 1000f, 1000f, 0.001f, new InputButton(Buttons[5]), new InputButton()),
			new InputAxe("AttackA", 1000f, 1000f, 0.001f, new InputButton(Buttons[6]), new InputButton()),
			new InputAxe("AttackB", 1000f, 1000f, 0.001f, new InputButton(Buttons[7]), new InputButton())
		};
	}

	public static void LoadSettings(string filepath)
	{
		FileInfo fileInf = new FileInfo (filepath);
		if (!fileInf.Exists)
		{
			LoadInitSettings();
			return;
		}

		Axes = new List<InputAxe> (){ };
		XmlDocument xDoc = new XmlDocument ();
		xDoc.Load (filepath);
		XmlElement xRoot = xDoc.DocumentElement;
		if (xRoot.Name != "Axes")
			return;
		foreach (XmlNode xAxe in xRoot) {
			InputAxe newAxe = new InputAxe ();
			if (xAxe.Name == "Axe") {
				newAxe = InputAxe.Deserialize (xAxe);
				Axes.Add (newAxe);
			}
		}

		Buttons = new List<InputButton>(){};
		foreach(var cur in Axes) {
			if (!string.IsNullOrWhiteSpace (cur.Positive.Name))
				Buttons.Add (cur.Positive);

			if (!string.IsNullOrWhiteSpace (cur.Negative.Name))
				Buttons.Add (cur.Negative);
		}
	}

	public static void SaveSettings(string filepath)
	{
		XmlDocument xDoc = new XmlDocument ();
		XmlNode docNode = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
		xDoc.AppendChild (docNode);
		XmlNode xRoot = xDoc.CreateElement ("Axes");
		xDoc.AppendChild (xRoot);
		foreach(var cur in Axes)
		{
			XmlNode axeNode = xDoc.CreateElement("Axe");
			cur.Serialize (ref axeNode, ref xDoc);
			xRoot.AppendChild (axeNode);
		}
		xDoc.Save (filepath);
	}

	public static string GetButtonDescription(string buttonName)
	{
		string name = buttonName.ToLower ();
		if(name == "example")
			return "Example";
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name)
				return cur.Description;
		throw new System.ArgumentOutOfRangeException (buttonName);
	}

	public static string GetButtonFromString(string buttonName, bool IsMain)
	{
		string name = buttonName.ToLower ();
		if(name == "example")
			return "Example";
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name)
				if(IsMain)
					return cur.MainKey.ToString();
				else
					return cur.AltKey.ToString();
		throw new System.ArgumentOutOfRangeException (buttonName);
	}

	static void DeleteDuplicateKeys(string buttonName, KeyCode newKey)
	{
		string name = buttonName.ToLower ();
		foreach (InputButton cur in Buttons) {
			if (cur.Name.ToLower () != name) {
				if (cur.MainKey == newKey)
					cur.MainKey = KeyCode.None;
				if (cur.AltKey == newKey)
					cur.AltKey = KeyCode.None;
			}
		}
	}

	public static void ChangeButtonMainKey(string buttonName, KeyCode newKey)
	{
		bool isFoundButton = false;
		InputButton targetButton = new InputButton ();
		string name = buttonName.ToLower ();
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name) {
				isFoundButton = true;
				targetButton = cur;
			}
		if (!isFoundButton) {
			throw new System.ArgumentOutOfRangeException (buttonName);
		}
		targetButton.MainKey = newKey;
		DeleteDuplicateKeys (buttonName, newKey);
	}

	public static void ChangeButtonAltKey(string buttonName, KeyCode newKey)
	{
		bool isFoundButton = false;
		InputButton targetButton = new InputButton ();
		string name = buttonName.ToLower ();
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name) {
				isFoundButton = true;
				targetButton = cur;
			}
		if (!isFoundButton) {
			throw new System.ArgumentOutOfRangeException (buttonName);
		}
		targetButton.AltKey = newKey;
		DeleteDuplicateKeys (buttonName, newKey);
	}

	public static float GetAxis(string axeName)
	{
		string name = axeName.ToLower ();
		foreach (InputAxe cur in Axes)
			if (cur.Name.ToLower () == name)
				return cur.Value;
		throw new System.ArgumentOutOfRangeException (axeName);
	}

	public static bool GetButton(string buttonName)
	{
		string name = buttonName.ToLower ();
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name)
				return cur.IsPressed;
		throw new System.ArgumentOutOfRangeException (buttonName);
	}

	public static bool GetButtonDown(string buttonName)
	{
		string name = buttonName.ToLower ();
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name)
				return cur.IsDown;
		Debug.Log (buttonName);
		throw new System.ArgumentOutOfRangeException ();
	}

	public static bool GetButtonUp(string buttonName)
	{
		string name = buttonName.ToLower ();
		foreach (InputButton cur in Buttons)
			if (cur.Name.ToLower () == name)
				return cur.IsUp;
		Debug.Log (buttonName);
		throw new System.ArgumentOutOfRangeException ();
	}

	public static bool GetKey(KeyCode key)
	{
		return Input.GetKey(key);
	}

	public static bool GetKeyDown(KeyCode key)
	{
		return Input.GetKeyDown(key);
	}

	public static bool GetKeyUp(KeyCode key)
	{
		return Input.GetKeyUp(key);
	}

	public static bool GetMouseButton(int button)
	{
		return Input.GetMouseButton(button);
	}

	public static bool GetMouseButtonDown(int button)
	{
		return Input.GetMouseButtonDown(button);
	}

	public static bool GetMouseButtonUp(int button)
	{
		return Input.GetMouseButtonUp(button);
	}
}
