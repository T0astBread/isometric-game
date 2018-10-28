using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Vector2Int step = Vector2Int.zero;

		float movement = GetAxisMovement(animator, "x");
		if (movement == 0)
		{
			movement = GetAxisMovement(animator, "y");
			step = Vector2Int.down;
		}
		else
		{
			step = Vector2Int.right;
		}
		step *= (int)Mathf.Sign(movement);

		Entity entity = animator.GetComponent<Entity>();
		Vector2Int mapDestination = entity.mapPosition + step;
		bool destinationTileExists = Map.Tiles.ContainsKey(mapDestination);

		IEnumerator movementCoroutine = null;
		if (destinationTileExists)
		{
			animator.SetFloat("movement_speed_multiplier", 1);
			movementCoroutine = entity.StepTo(mapDestination, Vector2.zero);
		}
		else if (entity is Player)
		{
			Player player = entity as Player;
			animator.SetFloat("movement_speed_multiplier", 1 / player.blockedStepTime / 2);
			movementCoroutine = player.StepBlockedTo(mapDestination);
		}
		if (movementCoroutine != null)
		{
			entity.StartCoroutine(movementCoroutine);
		}
	}

	private float GetAxisMovement(Animator animator, string axisName)
	{
		axisName = "movement_input_" + axisName;
		float movement = animator.GetFloat(axisName);
		if (movement != 0) movement = Mathf.Sign(movement);
		animator.SetFloat(axisName, 0);
		return movement;
	}
}
