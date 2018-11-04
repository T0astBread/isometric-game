using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovementBehaviour))]
public class Enemy : Entity
{

	private JumpBehaviour jumpBehaviour;
	private LinearMovementBehaviour moveBehaviour;
	private DirectionalAnimationControllerBehaviour directionalAnimationBehaviour;

	override public void Start()
	{
		base.Start();
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();
		this.directionalAnimationBehaviour = GetComponent<DirectionalAnimationControllerBehaviour>();
	}

	public override IEnumerator ExecuteStepTo(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		yield return new WaitForSeconds(.14f);
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.stepTime));
		yield return this.moveBehaviour.Move(newMapPosition, offsetInTile, this.stepTime);
	}

	protected override void BeforeSideStep()
	{
		this.directionalAnimationBehaviour.SetControllerDirection(-this.offsetInTileWhenMovingToSide);
		this.directionalAnimationBehaviour.enabled = false;
	}

	protected override void AfterSideStep()
	{
		this.directionalAnimationBehaviour.enabled = true;
	}
}
