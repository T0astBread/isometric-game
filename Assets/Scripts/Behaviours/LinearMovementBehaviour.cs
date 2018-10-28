using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovementBehaviour : MonoBehaviour
{
	private DirectionalSpriteBehaviour directionalSpriteBehaviour;

	void Start()
	{
		this.directionalSpriteBehaviour = GetComponentInChildren<DirectionalSpriteBehaviour>();
	}

	public IEnumerator Move(Vector2 newMapPosition, Vector2 offsetInTile, float duration, bool changeDirectionalSprite = true)
	{
		Vector3 endPos = Map.ConvertToWorldPosition(newMapPosition) + offsetInTile;
		Vector3 startPos = transform.position;
		Vector3 movement = endPos - startPos;

		if(changeDirectionalSprite && this.directionalSpriteBehaviour != null)
		{
			this.directionalSpriteBehaviour.SetSpriteDirection(movement);
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
	}
}