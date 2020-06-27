using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class CustomAnimationEvent : StateMachineBehaviour
{
    [System.Serializable]
    public class StateEvent
    {
        public string eventName = "Event";

        [Range(0, 1)]
        public float time = 0;

        public bool isFired { private set; get; } = false;
        public void Fire(Animator animator, AnimatorStateInfo stateInfo, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
        {
            if (!isFired) {
                Debug.Log("FIRE");
                animator.GetComponentInParent<PlayerManager>().gameObject.SendMessage(eventName, stateInfo, options);
                isFired = true;
            }
        }
        
        public void Reset()
        {
            isFired = false;
        }
    }
    
    public SendMessageOptions options = SendMessageOptions.DontRequireReceiver;

    public List<string> onStateEnterEvent = new List<string>();

    public List<string> onStateExitEvent = new List<string>();
    
    public List<StateEvent> onTimeEvent = new List<StateEvent>();

    private float time = 0;

    private float previous = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateEnterEvent.ForEach((x) => {
            animator.GetComponentInParent<PlayerManager>().gameObject.SendMessage(x, stateInfo, options);
        });
        time = previous = 0;
        
        onTimeEvent.ForEach((item) => {
            item.Reset();
        });
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float current = stateInfo.normalizedTime - time;
        if (current > 1) {
            time++;
        }

        if (current > 1 || previous > current) {
            onTimeEvent.ForEach((item) => {
                item.Reset();
            });
        }

        foreach (var timeEvent in onTimeEvent) {
            if (current >= timeEvent.time) {
                timeEvent.Fire(animator, stateInfo, options);
            }
        }

        previous = current;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateExitEvent.ForEach((x) => {
            animator.GetComponentInParent<PlayerManager>().gameObject.SendMessage(x, stateInfo, options);
        });
    }
}