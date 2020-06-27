using Managers;
using UnityEngine;
using UnityEngine.Animations;

namespace Utility.Animator
{
    public class SimpleEventDispatcher : StateMachineBehaviour
    {
        public string eventName;
        public bool onStart, onEnd;

        public string currentAnimation = "";
        
        private bool isStarted = false;
        public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            isStarted = true;
            if (onStart) {
                animator.GetComponentInParent<PlayerManager>().gameObject.SendMessage(eventName, true, SendMessageOptions.DontRequireReceiver);
            }
        }

        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onEnd && isStarted && (currentAnimation == "" || stateInfo.IsName(currentAnimation))) {
                animator.GetComponentInParent<PlayerManager>().gameObject.SendMessage(eventName, false, SendMessageOptions.DontRequireReceiver);
            }
            isStarted = false;
        }
    }
}
