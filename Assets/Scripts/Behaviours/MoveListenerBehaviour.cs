using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveListenerBehaviour : MonoBehaviour
{
	public virtual void BeforeMove(Vector2 movement)
	{
	}

	public virtual void AfterMove(Vector2 movement)
	{
	}
}
