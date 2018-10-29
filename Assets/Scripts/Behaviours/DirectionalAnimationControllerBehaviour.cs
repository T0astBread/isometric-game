using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAnimationControllerBehaviour : MoveListenerBehaviour
{
	public RuntimeAnimatorController northEastController, southEastController, southWestController, northWestController;

	private Animator animator;

	void Start()
	{
		this.animator = GetComponent<Animator>();
	}

	override public void BeforeMove(Vector2 movement)
	{
		SetControllerDirection(movement);
	}

	public void SetControllerDirection(Vector2 direction)
	{
		RuntimeAnimatorController controller = null;
		if (direction.y > 0)
		{
			if (direction.x > 0)
				controller = this.northEastController;
			else
				controller = this.northWestController;
		}
		else
		{
			if (direction.x > 0)
				controller = this.southEastController;
			else
				controller = this.southWestController;
		}
		this.animator.runtimeAnimatorController = controller;
	}
}
