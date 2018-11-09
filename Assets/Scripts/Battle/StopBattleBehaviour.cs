using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopBattleBehaviour : MonoBehaviour
{
	public void StopBattle()
	{
		Debug.Log("Stopping battle...");
		LevelObject.SetAllActive(true);
		SceneManager.UnloadSceneAsync("BattleScene");
	}
}
