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
	private DirectionalSpriteBehaviour directionalSpriteBehaviour;

	private bool freezeDirectionalSprite = false;

	override public void Start()
	{
		base.Start();
		this.animator = GetComponent<Animator>();
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();
		this.directionalSpriteBehaviour = GetComponentInChildren<DirectionalSpriteBehaviour>();
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

	public override IEnumerator ExecuteStepTo(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.stepTime));
		yield return this.moveBehaviour.Move(newMapPosition, offsetInTile, this.stepTime);
	}

	protected override void BeforeSideStep()
	{
		this.directionalSpriteBehaviour.SetSpriteDirection(-this.offsetInTileWhenMovingToSide);
		this.directionalSpriteBehaviour.enabled = false;
	}

	protected override void AfterSideStep()
	{
		this.directionalSpriteBehaviour.enabled = true;
	}

	public IEnumerator StepBlockedTo(Vector2Int newMapPosition)
	{
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.blockedStepTime));

		Vector3 worldMovement = (((Vector3)Map.ConvertToWorldPosition(newMapPosition)) - transform.position) * this.blockedStepXMovement;
		for (int i = 1; i >= 0; i--)
		{
			this.directionalSpriteBehaviour.enabled = i == 1;
			yield return this.moveBehaviour.Move(this.mapPosition, i * worldMovement, this.blockedStepTime / 2);
		}
		this.directionalSpriteBehaviour.enabled = true;
	}
}
