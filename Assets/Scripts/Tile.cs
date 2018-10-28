using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;

public class Tile : MonoBehaviour
{
	void Start()
	{
		RectangleObject tileObject = GetComponent<RectangleObject>();
		Vector2Int tileMapPosition = Vector2Int.FloorToInt(tileObject.TmxPosition / tileObject.TmxSize);
		Map.Tiles[tileMapPosition] = this;
	}
}
