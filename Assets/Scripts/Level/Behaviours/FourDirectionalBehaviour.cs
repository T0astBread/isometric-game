using System;
using UnityEngine;

public abstract class FourDirectionalBehaviour<T> : DirectionalBehaviour
{
	public T northEastItem, southEastItem, southWestItem, northWestItem;

	private T[] items;

	public virtual void Start()
	{
		this.items = new T[] {
			this.northEastItem,
			this.northWestItem,
			this.southEastItem,
			this.southWestItem
		};
	}

	override protected void _SetDirection(Vector2 direction)
	{
		int directionIndex = ((direction.y > 0 ? 0 : 1) << 1) | (direction.x > 0 ? 0 : 1);
		ApplyDirectionalItem(this.items[directionIndex]);
	}

	protected abstract void ApplyDirectionalItem(T directionalItem);
}