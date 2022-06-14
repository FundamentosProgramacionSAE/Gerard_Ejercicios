using System;
using AI.Manager;
using UnityEngine;

namespace Managers
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator Animator;
        public bool CanRotate;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting,bool canRotate = false)
        {
            Animator.applyRootMotion = isInteracting;
            Animator.SetBool("canRotate", canRotate);
            Animator.SetBool("isInteracting", isInteracting);
            Animator.CrossFade(targetAnim, 0.2f);
        }
        
        public void EnableAreaDamage()
        {
            Animator.SetBool("IsAreaDamage", true);
        }
    }
}