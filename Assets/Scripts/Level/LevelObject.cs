using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
	public static List<LevelObject> CurrentLOBs { get; private set; }

	private bool savedActiveState;

	static LevelObject()
	{
		CurrentLOBs = new List<LevelObject>();
	}

	void Start()
	{
		CurrentLOBs.Add(this);
	}

	public static void SetAllActive(bool active)
	{
		CurrentLOBs.ForEach(lob => {
			if (active)
			{
				lob.gameObject.SetActive(lob.savedActiveState);
			}
			else
			{
				lob.savedActiveState = lob.gameObject.activeSelf;
				lob.gameObject.SetActive(false);
			}
		});
	}

	public static void DropCurrentLOBs()
	{
		CurrentLOBs.Clear();
	}
}
