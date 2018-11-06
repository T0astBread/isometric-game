using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
	public float jumpHeight = .5f, jumpDuration = .5f;
	public bool resetPositionAfterJump = false;

	public IEnumerator Jump()
	{
		return Jump(this.jumpHeight, this.jumpDuration);
	}

	public IEnumerator Jump(float height, float duration)
	{
		float prevJumpY = 0;
		float currJumpY = 0;
		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			prevJumpY = currJumpY;
			currJumpY = JumpY(t / duration, height);
			float deltaJumpY = currJumpY - prevJumpY;
			transform.position += new Vector3(0, deltaJumpY, 0);
			yield return null;
		}

		if (this.resetPositionAfterJump)
		{
			transform.localPosition = Vector3.zero;
		}
	}

	private float JumpY(float t, float jumpHeight)
	{
		return Mathf.Clamp((-Mathf.Pow(4 * t - 2, 2) + 4) / 4 * jumpHeight, 0, jumpHeight);
	}
}