using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSpriteBehaviour : DirectionalBehaviour
{
	public const int SOUTH_EAST = 0, SOUTH_WEST = 1, NORTH_EAST = 2, NORTH_WEST = 3;

	public Sprite[] directionalSprites;  // north east, south east, south west, north west

	private SpriteRenderer spriteRenderer;

	void Start()
	{
		this.spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetSpriteDirection(int directionIndex)
	{
		this.spriteRenderer.sprite = this.directionalSprites[directionIndex];
	}

	override protected void _SetDirection(Vector2 direction)
	{
		int directionIndex = ((direction.y > 0 ? 1 : 0) << 1) | (direction.x > 0 ? 0 : 1);
		SetSpriteDirection(directionIndex);
	}
}
