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

		Tile destinationTile;
		int entitiesOnTile = 0;
		if (Map.Tiles.TryGetValue(mapDestination, out destinationTile))
		{
			entitiesOnTile = destinationTile.EntitiesOnTile.Count;
		}

		IEnumerator movementCoroutine = null;
		if (destinationTile != null && entitiesOnTile < 2)
		{
			animator.SetFloat("movement_speed_multiplier", 1);

			bool destinationTileIsEmpty = entitiesOnTile <= 0;
			if (destinationTileIsEmpty)
			{
				movementCoroutine = entity.StepTo(mapDestination, Vector2.zero);
			}
			else
			{
				Vector2 ownOffset = (Map.ConvertToWorldPosition(mapDestination) - (Vector2)animator.transform.position) * -entity.offsetDistanceWhenMovingToSide;
				destinationTile.EntitiesOnTile.ForEach(opposingEntity => {
					opposingEntity.offsetInTileWhenMovingToSide = -ownOffset.normalized * opposingEntity.offsetDistanceWhenMovingToSide;
				});
				movementCoroutine = entity.StepTo(mapDestination, ownOffset);
			}
			entity.isAtSideOfTile = !destinationTileIsEmpty;
		}
		else if (entity is Player && !entity.isAtSideOfTile)
		{
			Player player = entity as Player;
			animator.SetFloat("movement_speed_multiplier", 1 / player.blockedStepTime / 2);
			movementCoroutine = player.StepBlockedTo(mapDestination);
		}
		else
		{
			animator.SetTrigger("cancel_movement");
		}
		if (movementCoroutine != null)
		{
			entity.StartCoroutine(movementCoroutine);
		}
	}

	private float GetAxisMovement(Animator animator, string axisName)
	{
		axisName = "movement_input_" + axisName;
		float movement = animator.GetInteger(axisName);
		if (movement != 0) movement = Mathf.Sign(movement);
		animator.SetInteger(axisName, 0);
		return movement;
	}
}
