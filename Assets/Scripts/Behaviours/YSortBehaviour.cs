using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSortBehaviour : MonoBehaviour
{
	private const int POSITION_MULTIPLIER = 1000;

	private SpriteRenderer spriteRenderer;

	void Start()
	{
		this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	void Update()
	{
		this.spriteRenderer.sortingOrder = -(int)(transform.position.y * POSITION_MULTIPLIER);
	}
}
