using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public float stepTime = .5f;
	public Vector2Int mapPosition;
	public float offsetDistanceWhenMovingToSide = .35f;
	
	[HideInInspector]
	public int movementLocks = 0;
	[HideInInspector]
	public Tile positionTile;
	[HideInInspector]
	public bool isAtSideOfTile = false;
	[HideInInspector]
	public Vector2 offsetInTileWhenMovingToSide;

	protected DirectionalBehaviour[] directionalBehaviours;
	private IMovementRestriction[] movementRestrictions;

	public virtual void Start()
	{
		this.directionalBehaviours = GetComponentsInChildren<DirectionalBehaviour>();
		this.movementRestrictions = GetComponents<IMovementRestriction>();
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

	public bool CanMoveTo(Vector2Int mapPosition, Tile destinationTile)
	{
		foreach (var movementRestriction in this.movementRestrictions)
		{
			if (!movementRestriction.Allows(mapPosition, destinationTile))
				return false;
		}
		return true;
	}

	public IEnumerator StepTo(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		if (this.movementLocks <= 0)
		{
			this.movementLocks++;
			UnregisterMapPosition();

			Vector3 movement = CalculateTotalMovement(newMapPosition, offsetInTile);
			SetDirection(movement);
			yield return _StepTo(newMapPosition, offsetInTile, movement);

			RegisterNewMapPosition(newMapPosition);
			this.movementLocks--;
		}
	}

	protected Vector3 CalculateTotalMovement(Vector2Int newMapPosition, Vector2 offsetInTile)
	{
		Vector3 endPos = Map.ConvertToWorldPosition(newMapPosition) + offsetInTile;
		Vector3 startPos = transform.position;
		return endPos - startPos;
	}

	protected abstract IEnumerator _StepTo(Vector2Int newMapPosition, Vector2 offsetInTile, Vector2 totalMovement);

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

	public IEnumerator StepToSideOfTile()
	{
		if (this.movementLocks <= 0)
		{
			this.movementLocks++;
			this.isAtSideOfTile = true;
			BeforeSideStep();
			Vector3 movement = this.offsetInTileWhenMovingToSide;
			yield return _StepTo(this.mapPosition, this.offsetInTileWhenMovingToSide, movement);
			AfterSideStep();
			this.movementLocks--;
		}
	}

	protected virtual void BeforeSideStep()
	{
		foreach (var directionalBehaviour in this.directionalBehaviours)
		{
			directionalBehaviour.SetDirection(-this.offsetInTileWhenMovingToSide);
			directionalBehaviour.enabled = false;
		}
	}

	protected virtual void AfterSideStep()
	{
		foreach (var directionalBehaviour in this.directionalBehaviours)
		{
			directionalBehaviour.enabled = true;
		}
	}

	private IEnumerator StepToCenterOfTile()
	{
		if (this.movementLocks <= 0)
		{
			this.movementLocks++;
			this.isAtSideOfTile = false;
			Vector3 movement = CalculateTotalMovement(this.mapPosition, Vector2.zero);
			yield return _StepTo(this.mapPosition, Vector2.zero, movement);
			this.movementLocks--;
		}
	}

	public void SetDirection(Vector2 direction)
	{
		foreach (var directionalBehaviour in this.directionalBehaviours)
		{
			directionalBehaviour.SetDirection(direction);
		}
	}
}
