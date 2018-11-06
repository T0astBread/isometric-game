using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementRestriction
{
	bool Allows(Vector2Int newMapPosition, Tile destinationTile);
}
