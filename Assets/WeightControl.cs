using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Nemesh.Logger;

public class WeightControl : StateMachineBehaviour
{
    [SerializeField]
    private float weightHigh = 1f;
    [SerializeField]
    private float blendTime = 0.4f;

    private bool _ending;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, weightHigh);
        _ending = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 0f);
        _ending = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_ending && stateInfo.normalizedTime >= blendTime)
        {
            var t = Mathf.Lerp(weightHigh, 0f, stateInfo.normalizedTime - blendTime);
            animator.SetLayerWeight(layerIndex, t);
        }
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
