using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	private Animator animator;

	// Use this for initialization
	void Start()
	{
		this.animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		HandleMovement("Horizontal", "movement_x");
		HandleMovement("Vertical", "movement_y");
	}

	private void HandleMovement(string axisName, string animatorParameterName)
	{
		float movement = Input.GetAxisRaw(axisName);
		this.animator.SetFloat(animatorParameterName, movement);
	}
}
