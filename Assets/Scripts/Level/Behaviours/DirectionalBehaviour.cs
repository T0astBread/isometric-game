using System;
using UnityEngine;

public abstract class DirectionalBehaviour : MonoBehaviour
{
	public void SetDirection(Vector2 direction)
	{
		if (this.enabled)
			this._SetDirection(direction);
	}

	protected abstract void _SetDirection(Vector2 direction);
}