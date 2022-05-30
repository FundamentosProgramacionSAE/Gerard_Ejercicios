using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnExit : StateMachineBehaviour
{
    public string TargetBool;
    public bool Status;
    

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(TargetBool, Status);
    }
}