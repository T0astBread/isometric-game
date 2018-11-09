using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class RandomWalkBehaviour : MonoBehaviour
{
	public float minTimeBetweenWalks, maxTimeBetweenWalks;

	private Animator animator;

	void OnEnable()
	{
		this.animator = GetComponent<Animator>();
		StartCoroutine(WalkForever());
	}

	private IEnumerator WalkForever()
	{
		int layerIndex = this.animator.GetLayerIndex("Base Layer");
		while (true)
		{
			yield return new WaitForSeconds(GenerateTimeUntilNextWalk());

			AnimatorStateInfo currentState = this.animator.GetCurrentAnimatorStateInfo(layerIndex);
			yield return new WaitWhile(() => !this.enabled || !currentState.IsName("Idle") || this.animator.IsInTransition(layerIndex));

			WalkRandomly();
		}
	}

	private float GenerateTimeUntilNextWalk()
	{
		return Random.Range(this.minTimeBetweenWalks, this.maxTimeBetweenWalks);
	}

	private void WalkRandomly()
	{
		float r = Random.value;

		Vector2Int step = Vector2Int.up;
		if (r < .25f)
			step = Vector2Int.right;
		else if (r < .5f)
			step = Vector2Int.down;
		else if (r < .75f)
			step = Vector2Int.left;

		this.animator.SetInteger("movement_input_x", step.x);
		this.animator.SetInteger("movement_input_y", step.y);
	}
}
