using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovementBehaviour : MonoBehaviour
{
	private MoveListenerBehaviour[] listeners;

	void Start()
	{
		this.listeners = GetComponentsInChildren<MoveListenerBehaviour>();
	}

	public IEnumerator Move(Vector2 newMapPosition, Vector2 offsetInTile, float duration)
	{
		Vector3 endPos = Map.ConvertToWorldPosition(newMapPosition) + offsetInTile;
		Vector3 startPos = transform.position;
		Vector3 movement = endPos - startPos;

		foreach (var listener in this.listeners)
		{
			listener.BeforeMove(movement);
		}

		Vector3 prevMovement = Vector3.zero;
		Vector3 currMovement = Vector3.zero;
		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			prevMovement = currMovement;
			currMovement = movement * (t / duration);
			Vector3 movementDelta = currMovement - prevMovement;

			transform.position += movementDelta;
			yield return null;
		}

		foreach (var listener in this.listeners)
		{
			listener.AfterMove(movement);
		}
	}
}