using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class HomeAreaMovementRestriction : MonoBehaviour, IMovementRestriction
{
	public Vector2Int home;
	public int homeAreaRadius;

	private Entity entity;

	void Start()
	{
		this.entity = GetComponent<Entity>();
	}

	public bool Allows(Vector2Int newMapPosition, Tile destinationTile)
	{
		int distanceFromHome = Mathf.Abs(Mathf.CeilToInt((newMapPosition - this.home).magnitude));
		return distanceFromHome <= this.homeAreaRadius;
	}
}
