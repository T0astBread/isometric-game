using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovementBehaviour))]
public class Enemy : Entity
{
	private JumpBehaviour jumpBehaviour;
	private LinearMovementBehaviour moveBehaviour;

	override public void Start()
	{
		base.Start();
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();
	}

	protected override IEnumerator _StepTo(Vector2Int newMapPosition, Vector2 offsetInTile, Vector2 totalMovement)
	{
		yield return new WaitForSeconds(.14f);
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.stepTime));
		yield return this.moveBehaviour.Move(totalMovement, this.stepTime);
	}
}
