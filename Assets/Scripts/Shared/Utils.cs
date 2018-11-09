using System;
using UnityEngine;

public class Utils
{
	public static void SetAllGameObjectsActive(bool active)
	{
		foreach (var obj in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if (obj is GameObject)
			{
				GameObject gameObj = obj as GameObject;
				gameObj.SetActive(false);
			}
		}
	}
}