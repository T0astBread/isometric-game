using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : StateMachineBehaviour
{
	private const int NORTH_EAST = 0, SOUTH_EAST = 1, SOUTH_WEST = 2, NORTH_WEST = 3;

	public float stepSizeX = 1.575f;
	public float stepSizeY = .825f;
	public float jumpHeight = .5f;

	public Sprite[] directionalSprites;  // north east, south east, south west, north west

	private Vector2 movement;
	private float prevNormalizedTime;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.prevNormalizedTime = 0;

		int sprite;

		float yMovement = GetAxisMovement(animator, "y");
		float xMovement = 0;
		if (yMovement == 0)
		{
			xMovement = GetAxisMovement(animator, "x");
			this.movement = new Vector2(this.stepSizeX, -this.stepSizeY);
			sprite = SOUTH_EAST;
			if (xMovement < 0)
			{
				this.movement *= -1f;
				sprite = NORTH_WEST;
			}
		}
		else
		{
			this.movement = new Vector2(this.stepSizeX, this.stepSizeY);
			sprite = NORTH_EAST;
			if (yMovement < 0)
			{
				this.movement *= -1f;
				sprite = SOUTH_WEST;
			}
		}

		animator.GetComponent<SpriteRenderer>().sprite = this.directionalSprites[sprite];
	}

	private float GetAxisMovement(Animator animator, string axisName)
	{
		axisName = "movement_" + axisName;
		float movement = animator.GetFloat(axisName);
		if (movement != 0) movement = Mathf.Sign(movement);
		animator.SetFloat(axisName, 0);
		return movement;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.transform.position = RoundPosition(animator.transform.position);
	}

	private Vector3 RoundPosition(Vector3 v)
	{
		return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y * 2)/2, v.z);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float prevJumpY = JumpY(this.prevNormalizedTime);
		float currJumpY = JumpY(stateInfo.normalizedTime);
		float deltaJumpY = currJumpY - prevJumpY;

		float deltaTime = stateInfo.normalizedTime - this.prevNormalizedTime;
		this.prevNormalizedTime = stateInfo.normalizedTime;

		Vector2 scaledMovement = this.movement * deltaTime;
		Vector3 position = animator.transform.position;
		animator.transform.position = new Vector3(position.x + scaledMovement.x, position.y + scaledMovement.y + deltaJumpY, position.z);
	}

	private float JumpY(float t)
	{
		return Mathf.Clamp((-Mathf.Pow(4 * t - 2, 2) + 4) / 4 * this.jumpHeight, 0, this.jumpHeight);
	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
