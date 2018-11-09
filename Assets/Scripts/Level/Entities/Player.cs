using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovementBehaviour), typeof(JumpBehaviour))]
public class Player : Entity
{
	public float blockedStepTime = .25f, blockedStepXMovement = .1f;

	private Animator animator;
	private JumpBehaviour jumpBehaviour;
	private LinearMovementBehaviour moveBehaviour;
	private BattleBehaviour battleBehaviour;

	private bool freezeDirectionalSprite = false;

	override public void Start()
	{
		base.Start();
		this.animator = GetComponent<Animator>();
		this.jumpBehaviour = GetComponentInChildren<JumpBehaviour>();
		this.moveBehaviour = GetComponent<LinearMovementBehaviour>();
		this.battleBehaviour = GetComponent<BattleBehaviour>();

		// this.battleBehaviour.StartBattle(GameObject.Find("Enemy").GetComponent<Enemy>());
	}

	override public void OnEnable()
	{
		base.OnEnable();
		ResetPosition();
	}

	override public void Update()
	{
		base.Update();
		HandleMovement("Horizontal", "movement_input_x");
		HandleMovement("Vertical", "movement_input_y");

		if (Input.GetKeyDown(KeyCode.B))
		{
			this.battleBehaviour.StartBattle(GameObject.Find("Enemy").GetComponent<Enemy>());
		}
	}

	private void HandleMovement(string axisName, string animatorParameterName)
	{
		float movement = Input.GetAxisRaw(axisName);
		this.animator.SetInteger(animatorParameterName, Mathf.RoundToInt(movement));
	}

	override public void ResetPosition()
	{
		base.ResetPosition();
		transform.Find("SpriteWrapper").localPosition = Vector3.zero;
		this.animator.SetInteger("movement_input_x", 0);
		this.animator.SetInteger("movement_input_y", 0);
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
