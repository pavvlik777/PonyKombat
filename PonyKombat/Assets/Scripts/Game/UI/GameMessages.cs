using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_Game.Combat.UI
{
	public class GameMessages : MonoBehaviour
	{
		[SerializeField]private float timeToShowMessage = 1f;
		[SerializeField]private float timeToHideMessage = 5f;

		[SerializeField]private Text message = null;


		public void ShowMessage(string s)
		{
			message.text = s;
			StartCoroutine(_ShowMessage());
		}

		IEnumerator _ShowMessage()
		{
			float timePassed = 0f;
			Color tempColor = message.color;
			while(timePassed < timeToShowMessage)
			{
				timePassed += GameTime.deltaTime;
				if(timePassed >= timeToShowMessage)
					break;	
				tempColor.a = timePassed / timeToShowMessage;
				message.color = tempColor;
				yield return null;
			}
			tempColor.a = 1f;
			message.color = tempColor;
			StartCoroutine(_HideMessage());
			yield break;
		}

		IEnumerator _HideMessage()
		{
			float timePassed = timeToHideMessage;
			Color tempColor = message.color;
			while(timePassed > 0f)
			{
				timePassed -= GameTime.deltaTime;
				if(timePassed <= 0f)
					break;	
				tempColor.a = timePassed / timeToHideMessage;
				message.color = tempColor;
				yield return null;
			}
			tempColor.a = 0f;
			message.color = tempColor;
			yield break;
		}
	}
}
