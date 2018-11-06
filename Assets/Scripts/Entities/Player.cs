using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovementBehaviour))]
public class Player : Entity
{
	public float blockedStepTime = .25f, blockedStepXMovement = .1f;

	private Animator animator;
	private JumpBehaviour jumpBehaviour;
	private LinearMovementBehaviour moveBehaviour;

	private bool freezeDirectionalSprite = false;

	override public void Start()
	{
		base.Start();
		this.animator = GetComponent<Animator>();
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();
	}

	public override void Update()
	{
		base.Update();
		HandleMovement("Horizontal", "movement_input_x");
		HandleMovement("Vertical", "movement_input_y");
	}

	private void HandleMovement(string axisName, string animatorParameterName)
	{
		float movement = Input.GetAxisRaw(axisName);
		this.animator.SetInteger(animatorParameterName, Mathf.RoundToInt(movement));
	}

	protected override IEnumerator _StepTo(Vector2Int newMapPosition, Vector2 offsetInTile, Vector2 totalMovement)
	{
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.stepTime));
		yield return this.moveBehaviour.Move(totalMovement, this.stepTime);
	}

	public IEnumerator StepBlockedTo(Vector2Int newMapPosition)
	{
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.blockedStepTime));

		Vector3 worldMovement = (((Vector3)Map.ConvertToWorldPosition(newMapPosition)) - transform.position) * this.blockedStepXMovement;
		SetDirection(worldMovement);
		for (int i = 1; i >= 0; i--)
		{
			yield return this.moveBehaviour.Move(CalculateTotalMovement(this.mapPosition, i * worldMovement), this.blockedStepTime / 2);
		}
	}
}
