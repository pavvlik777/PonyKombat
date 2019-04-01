using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_MenuFSM
{
	public class VideoSettingsState : State
	{
		private List<Resolution> awailableResolution;

		[SerializeField]private UnityEngine.UI.Dropdown m_Dropdown = null;
		void Awake()
		{
			awailableResolution = new List<Resolution>(Screen.resolutions);
			CleanResolutionList(new Predicate<Resolution>[] {
				(t) => { return t.width < 800; }
			});
			m_Dropdown.AddOptions(GetOptionsList(awailableResolution));
			try
			{
				m_Dropdown.value = GetResolutionIndex(GameVideo.ScreenResolution.width);
			}
			catch (Exception)
            {
				Debug.Log("Troubles with resolution");
			}
		}

		void CleanResolutionList(Predicate<Resolution>[] conditions)
		{
			foreach(var cur in conditions)
				awailableResolution.RemoveAll(cur);
		}

		List<string> GetOptionsList(List<Resolution> list)
		{
			List<string> output = new List<string>(){};
			foreach(var cur in list)
				output.Add($"{cur.width}x{cur.height}");
			return output;
		}

		int GetResolutionIndex(int width)
		{
			for(int i = 0; i < awailableResolution.Count; i++)
				if(awailableResolution[i].width == width)
					return i;
			throw new ArgumentOutOfRangeException("There is no such resolution width");
		}

		public void ChangeCurrentResolution()
		{
			GameVideo.SetResolution(awailableResolution[m_Dropdown.value].width, awailableResolution[m_Dropdown.value].height);
		}
	}
}