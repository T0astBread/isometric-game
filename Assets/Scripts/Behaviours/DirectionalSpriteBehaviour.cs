using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSpriteBehaviour : FourDirectionalBehaviour<Sprite>
{
	private SpriteRenderer spriteRenderer;

	override public void Start()
	{
		base.Start();
		this.spriteRenderer = GetComponent<SpriteRenderer>();
	}

	override protected void ApplyDirectionalItem(Sprite directionalSprite)
	{
		this.spriteRenderer.sprite = directionalSprite;
	}
}
