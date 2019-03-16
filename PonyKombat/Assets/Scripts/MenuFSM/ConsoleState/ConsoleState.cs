using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_MenuFSM
{
	public class ConsoleState : State
	{
        [SerializeField]private GameObject MessageExample = null;
        [SerializeField]private ScrollRect scrollRect = null;
        [SerializeField]private InputField inputField = null;

        private int maxAmount = 10000;
        private List<GameObject> output = new List<GameObject>() {};
        private List<Text> outputText = new List<Text>() {};

        private float difY = 30f;

        public override void LeaveState(StatesNames newState)
        {
			SwitchStateObject (false);
        }

        public override void EnterState()
        {
			SwitchStateObject (true);
        }

        void Awake()
        {
            GameConsole.OnNewMessage += DisplayMessage;
            inputField.onEndEdit.AddListener(
                delegate {
                        if(Input.GetKey(KeyCode.Return))
                        {
                            InputMessage(inputField.text); 
                            inputField.text = "";
                        }
                    } );
        }

        void OnDestroy()
        {
            GameConsole.OnNewMessage -= DisplayMessage;
        }

        void DisplayMessage(string message)
        {
            if(output.Count < maxAmount)
            {
                MessageExample.SetActive (true);

                GameObject newObj = Instantiate(MessageExample, scrollRect.content);
                outputText.Add(newObj.GetComponent<Text> ());
                outputText[outputText.Count - 1].text = message;
                float x = MessageExample.GetComponent<RectTransform> ().localPosition.x;
                float y = MessageExample.GetComponent<RectTransform> ().localPosition.y - output.Count * difY;
                newObj.GetComponent<RectTransform> ().localPosition = new Vector3 (x, y);
                output.Add(newObj);
                    
                scrollRect.content.sizeDelta = new Vector2 (scrollRect.content.sizeDelta.x, 10 + (output.Count-1) * (difY - 30) + output.Count * 30); //2 * 5 + i * 40 + (i-1)*5
                scrollRect.verticalNormalizedPosition = 0f;
                MessageExample.SetActive (false);
            }
            else
            {
                for(int i = 0; i < maxAmount - 1; i++)
                    outputText[i].text = outputText[i + 1].text;
                outputText[maxAmount - 1].text = message;
            }
        }

        public void InputMessage(string s)
        {
            GameConsole.AddMessage(s, true);
        }
    }
}