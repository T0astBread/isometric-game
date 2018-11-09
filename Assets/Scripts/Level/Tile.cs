using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;

public class Tile : MonoBehaviour
{
	public List<Entity> EntitiesOnTile { get; private set; }

	void Start()
	{
		this.EntitiesOnTile = new List<Entity>();

		RectangleObject tileObject = GetComponent<RectangleObject>();
		Vector2Int tileMapPosition = Vector2Int.FloorToInt(tileObject.TmxPosition / tileObject.TmxSize);
		Map.Tiles[tileMapPosition] = this;
	}
}
