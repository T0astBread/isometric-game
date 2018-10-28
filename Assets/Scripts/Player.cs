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

	// Use this for initialization
	void Start()
	{
		this.animator = GetComponent<Animator>();
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();

		this.mapPosition = new Vector2Int(1, 16);
	}

	// Update is called once per frame
	void Update()
	{
		HandleMovement("Horizontal", "movement_input_x");
		HandleMovement("Vertical", "movement_input_y");
	}

	private void HandleMovement(string axisName, string animatorParameterName)
	{
		float movement = Input.GetAxisRaw(axisName);
		this.animator.SetFloat(animatorParameterName, movement);
	}

	public override IEnumerator StepTo(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.stepTime));
		yield return this.moveBehaviour.Move(newMapPosition, offsetInTile, this.stepTime);
		this.mapPosition = newMapPosition;
	}

	public IEnumerator StepBlockedTo(Vector2Int newMapPosition)
	{
		StartCoroutine(this.jumpBehaviour.Jump(this.jumpBehaviour.jumpHeight, this.blockedStepTime));

		Vector3 worldMovement = (((Vector3) Map.ConvertToWorldPosition(newMapPosition)) - transform.position) * this.blockedStepXMovement;
		for(int i = 1; i >= 0; i--)
		{
			yield return this.moveBehaviour.Move(this.mapPosition, i * worldMovement, this.blockedStepTime / 2, i == 1);
		}
	}
}
