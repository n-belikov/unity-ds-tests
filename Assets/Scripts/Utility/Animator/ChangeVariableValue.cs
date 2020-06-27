using UnityEngine;
using UnityEngine.Animations;

namespace Utility.Animator
{
    public class ChangeVariableValue : StateMachineBehaviour
    {
        public string variableName = "";
        public bool startValue = true;

        public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex, controller);

            animator.SetBool(variableName, startValue);
        }

        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateExit(animator, stateInfo, layerIndex, controller);
            animator.SetBool(variableName, !startValue);
        }
    }
}