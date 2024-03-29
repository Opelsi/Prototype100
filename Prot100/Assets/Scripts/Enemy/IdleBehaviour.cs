﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IdleBehaviour : StateMachineBehaviour
{
	WeaponController weaponController;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		weaponController = animator.gameObject.transform.GetComponent<WeaponController>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		if (animator.GetComponentInParent<EnemyMovement>().TargetIsFar())
		{
			animator.SetBool("IsFollow", true);
		}
		else
		{
			if(weaponController.canUse)
				animator.SetBool("IsAttack", true);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
