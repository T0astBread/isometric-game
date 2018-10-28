using System;
using System.Collections.Generic;
using UnityEngine;

class Map
{
	public const float stepSizeX = 1, stepSizeY = .5f;
	public static readonly Vector2 stepX = new Vector2(stepSizeX, -stepSizeY), stepY = new Vector2(-stepSizeX, -stepSizeY);
	
	public static IDictionary<Vector2Int, Tile> Tiles { get; private set; }

	static Map()
	{
		Tiles = new Dictionary<Vector2Int, Tile>();
	}
	
	public static Vector2 ConvertToWorldPosition(Vector2 mapPosition)
	{
		return mapPosition.x * stepX + mapPosition.y * stepY;
	}
	
	public static Vector3 RoundToGridPosition(Vector3 v)
	{
		return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y * 2) / 2, v.z);
	}
}