using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovementBehaviour : MonoBehaviour
{
	public IEnumerator Move(Vector2 movement, float duration)
	{
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