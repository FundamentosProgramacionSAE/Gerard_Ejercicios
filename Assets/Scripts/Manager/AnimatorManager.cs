using UnityEngine;

namespace Managers
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator Animator;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            Animator.applyRootMotion = isInteracting;
            Animator.SetBool("isInteracting", isInteracting);
            Animator.CrossFade(targetAnim, 0.2f);
        }
    }
}