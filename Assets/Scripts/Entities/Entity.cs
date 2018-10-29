using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public float stepTime = .5f;
	public Vector2Int mapPosition = Vector2Int.zero;

	public abstract IEnumerator StepTo(Vector2Int newMapPosition, Vector2 offsetInTile);
}
