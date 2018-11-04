using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public float stepTime = .5f;
	public int movementLocks = 0;
	public Vector2Int mapPosition;
	public Tile positionTile;
	public bool isAtSideOfTile = false;
	public Vector2 offsetInTileWhenMovingToSide;
	public float offsetDistanceWhenMovingToSide = .35f;

	public IEnumerator StepTo(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		if (this.movementLocks <= 0)
		{
			this.movementLocks++;
			UnregisterMapPosition();
			yield return ExecuteStepTo(newMapPosition, offsetInTile);
			RegisterNewMapPosition(newMapPosition);
			this.movementLocks--;
		}
	}

	public abstract IEnumerator ExecuteStepTo(Vector2Int newMapPosition, Vector2 offsetInTile);

	private void UnregisterMapPosition()
	{
		if (this.positionTile != null)
		{
			this.positionTile.EntitiesOnTile.Remove(this);
		}
	}

	private void RegisterNewMapPosition(Vector2Int newMapPosition)
	{
		Tile newPositionTile;
		if (Map.Tiles.TryGetValue(newMapPosition, out newPositionTile))
		{
			this.positionTile = newPositionTile;
			this.positionTile.EntitiesOnTile.Add(this);
		}
		this.mapPosition = newMapPosition;
	}

	public virtual void Start()
	{
		RegisterNewMapPosition(this.mapPosition);
	}

	public virtual void Update()
	{
		if (this.isAtSideOfTile)
		{
			if (this.positionTile != null && this.positionTile.EntitiesOnTile.Count <= 1)
			{
				StartCoroutine(StepToCenterOfTile());
			}
		}
		else if (this.positionTile != null && this.positionTile.EntitiesOnTile.Count > 1 && this.offsetInTileWhenMovingToSide != null)
		{
			StartCoroutine(StepToSideOfTile());
		}
	}

	public IEnumerator StepToSideOfTile()
	{
		if (this.movementLocks <= 0)
		{
			this.movementLocks++;
			this.isAtSideOfTile = true;
			BeforeSideStep();
			yield return ExecuteStepTo(this.mapPosition, this.offsetInTileWhenMovingToSide);
			AfterSideStep();
			this.movementLocks--;
		}
	}

	protected virtual void BeforeSideStep() { }
	protected virtual void AfterSideStep() { }

	private IEnumerator StepToCenterOfTile()
	{
		if (this.movementLocks <= 0)
		{
			this.movementLocks++;
			this.isAtSideOfTile = false;
			yield return ExecuteStepTo(this.mapPosition, Vector2.zero);
			this.movementLocks--;
		}
	}
}
