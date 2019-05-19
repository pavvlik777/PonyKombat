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

        int currentChosenCommand = -1;
        private List<string> commandsList = new List<string>{};
        private bool isActive = false;

        private float difY = 30f;

        public override void LeaveState(StatesNames newState)
        {
			SwitchStateObject (false);
            inputField.text = "";
            isActive = false;
        }

        public override void EnterState()
        {
			SwitchStateObject (true);
            isActive = true;
            currentChosenCommand = -1;
            inputField.text = "";
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
        void OnGUI()
        {
            if(isActive)
            {
                Event keyEvent = Event.current;
                if (keyEvent.isKey) {
                    if(keyEvent.keyCode != KeyCode.None && keyEvent.type == EventType.KeyDown)
                    {
                        inputField.Select();
                        inputField.ActivateInputField();
                        if(keyEvent.keyCode == KeyCode.UpArrow || keyEvent.keyCode == KeyCode.DownArrow)
                        {
                            ChooseCommand(keyEvent.keyCode);
                        }
                    }
                }
            }
        }

        void ChooseCommand(KeyCode arrow)
        {
            if(arrow == KeyCode.UpArrow)
            {
                if(currentChosenCommand < maxAmount - 1 && currentChosenCommand < commandsList.Count - 1)
                {
                    currentChosenCommand++;
                }
                if(currentChosenCommand == -1)
                    inputField.text = "";
                else
                    inputField.text = commandsList[currentChosenCommand];
            }
            else
            {
                if(currentChosenCommand > -1)
                {
                    currentChosenCommand--;
                }
                if(currentChosenCommand == -1)
                    inputField.text = "";
                else
                    inputField.text = commandsList[currentChosenCommand];
            }
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
            currentChosenCommand = -1;
            GameConsole.AddMessage(s, true);
            if(s != "")
                if(commandsList.Count < maxAmount)
                {
                    commandsList.Add(s);
                }
                else
                {
                    for(int i = 0; i < maxAmount - 1; i++)
                        commandsList[i] = commandsList[i + 1];
                    commandsList[maxAmount - 1] = s;
                }
        }
    }
}