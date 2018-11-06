using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Vector2Int step = GetMapStepForMovement(animator);

		Entity entity = animator.GetComponent<Entity>();
		Vector2Int mapDestination = entity.mapPosition + step;

		// Get the destination tile, if present
		Tile destinationTile;
		int entitiesOnTile = 0;
		if (Map.Tiles.TryGetValue(mapDestination, out destinationTile))
		{
			entitiesOnTile = destinationTile.EntitiesOnTile.Count;
		}

		// Get the right coroutine to use, if any
		IEnumerator movementCoroutine = null;
		if (destinationTile != null && entitiesOnTile < 2 && entity.CanMoveTo(mapDestination, destinationTile))
		{
			animator.SetFloat("movement_speed_multiplier", 1);

			bool destinationTileIsEmpty = entitiesOnTile <= 0;
			if (destinationTileIsEmpty)
			{
				movementCoroutine = entity.StepTo(mapDestination, Vector2.zero);
			}
			else
			{
				// Calculate the entity's own offset in the tile
				Vector2 ownOffset = (Map.ConvertToWorldPosition(mapDestination) - (Vector2)animator.transform.position) * -entity.offsetDistanceWhenMovingToSide;
				// Tell every other entity on the tile their new offset based on their desired offset distance
				destinationTile.EntitiesOnTile.ForEach(opposingEntity => {
					opposingEntity.offsetInTileWhenMovingToSide = -ownOffset.normalized * opposingEntity.offsetDistanceWhenMovingToSide;
				});
				movementCoroutine = entity.StepTo(mapDestination, ownOffset);
			}
			// If the destination tile isn't empty we're already jumping to the side
			entity.isAtSideOfTile = !destinationTileIsEmpty;
		}
		// If the player is not at the side of the tile (i.e. the tile is empty), make a blocked step
		// Blocked steps are forbidden on non-empty tiles due to glitchy behaviour
		else if (entity is Player && !entity.isAtSideOfTile)
		{
			Player player = entity as Player;
			animator.SetFloat("movement_speed_multiplier", 1 / player.blockedStepTime / 2);
			movementCoroutine = player.StepBlockedTo(mapDestination);
		}
		// If the entity can't make a blocked step, cancel the move state immediately
		else
		{
			animator.SetTrigger("cancel_movement");
		}
		if (movementCoroutine != null)
		{
			entity.StartCoroutine(movementCoroutine);
		}
	}

	private Vector2Int GetMapStepForMovement(Animator animator)
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
		return step;
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
