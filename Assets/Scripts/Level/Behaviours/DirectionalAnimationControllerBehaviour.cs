using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAnimationControllerBehaviour : FourDirectionalBehaviour<RuntimeAnimatorController>
{
	private Animator animator;

	override public void Start()
	{
		base.Start();
		this.animator = GetComponent<Animator>();
	}

	override protected void ApplyDirectionalItem(RuntimeAnimatorController directionalAnimationContoller)
	{
		this.animator.runtimeAnimatorController = directionalAnimationContoller;
	}
}
