using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovementBehaviour))]
public class Enemy : Entity
{

	private JumpBehaviour jumpBehaviour;
	private LinearMovementBehaviour moveBehaviour;
	private RuntimeAnimatorController runtimeAnimator;

	void Start()
	{
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();
		this.runtimeAnimator = GetComponent<Animator>().runtimeAnimatorController;
	}

	public override IEnumerator StepTo(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		yield return new WaitForSeconds(.14f);
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.stepTime));
		yield return this.moveBehaviour.Move(newMapPosition, offsetInTile, this.stepTime);
		this.mapPosition = newMapPosition;
	}
}
