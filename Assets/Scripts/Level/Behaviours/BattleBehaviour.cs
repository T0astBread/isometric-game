using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleBehaviour : MonoBehaviour
{
	public void StartBattle(Enemy enemy)
	{
		List<Enemy> enemies = new List<Enemy>(1);
		enemies.Add(enemy);
		StartBattle(enemies);
	}

	public void StartBattle(Enemy[] enemies)
	{
		StartBattle(new List<Enemy>(enemies));
	}

	public void StartBattle(List<Enemy> enemies)
	{
		Debug.Log("Loading battle...");
		LevelObject.SetAllActive(false);
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
	}
}
