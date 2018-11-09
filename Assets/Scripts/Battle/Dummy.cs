using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
	private float time;

	// Use this for initialization
	void Start()
	{
		Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update()
	{
		this.time += Time.deltaTime;
		transform.position = new Vector3(0, Time.time % 1, 0);
		if (this.time > 3)
		{
			this.time = 0;
			GetComponent<StopBattleBehaviour>().StopBattle();
		}
	}
}
