using System;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Locomotion
{
    public enum CharacterState
    {
        Sword, Bow, Default
    }

    public class CharacterAnimator : MonoBehaviour
    {
        [System.Serializable]
        public class Events
        {
            public UnityEvent onStartAttack;
            public UnityEvent onEndAttack;
            public UnityEvent onDrawArrow;
            public UnityEvent onBowShooting;
            public UnityEvent onUnDrawArrow;
        }

        [System.Serializable]
        public class AnimatorBodyWeight
        {
            [Range(0f, 1f)] public float weight = 0f, bodyWeight = 0f, headWeight = 0f;
        }

        private AnimatorBodyWeight BodyWeight = null;

        public Events events;

        [SerializeField] private Animator animator;
        [SerializeField] private CharacterState state = CharacterState.Default;

        private Vector3 _lockAtPosition = Vector3.zero;
        private CharacterMovement _movement;
        private float _horizontal, _vertical;

        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int OnGround = Animator.StringToHash("onGround");
        private static readonly int Motion = Animator.StringToHash("Motion");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int IsAiming = Animator.StringToHash("isAiming");

        private void Start()
        {
            if (!animator) {
                animator = GetComponent<Animator>();
            }

            _movement = GetComponentInParent<CharacterMovement>();
        }

        public CharacterState GetState()
        {
            return this.state;
        }

        public void SetState(CharacterState state)
        {
            this.state = state;
        }

        public void SetMotionValue(float value)
        {
            animator.SetFloat(Motion, value, 0.1f, Time.deltaTime);
        }

        public void SetMovementAxis(float horizontal, float vertical)
        {
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public void SetAttack(bool value)
        {
            animator.SetBool(IsAttack, value);
        }

        public void SetAiming(bool value)
        {
            animator.SetBool(IsAiming, value);
        }

        public void FireAnimation(string name, bool value, int? layerIndex = null)
        {
            if (layerIndex != null) {
                animator.CrossFadeInFixedTime(name, 0.1f, (int) layerIndex);
            }
            else {
                animator.CrossFadeInFixedTime(name, 0.1f);
            }
        }

        public void FireAnimation(string name, bool value, float duration, int? LayerIndex = null)
        {
            if (LayerIndex != null) {
                animator.CrossFadeInFixedTime(name, duration, (int) LayerIndex);
            }
            else {
                animator.CrossFadeInFixedTime(name, duration);
            }
        }

        public void SetIsJumping(bool isJumping)
        {
            animator.SetBool(IsJumping, isJumping);
        }

        public void SetGrounded(bool onGrounded)
        {
            animator.SetBool(OnGround, onGrounded);
        }

        public void MoveTick(float deltaTime)
        {
            animator.SetFloat(Horizontal, _horizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(Vertical, _vertical, 0.1f, Time.deltaTime);
        }

        public void StateTick(float deltaTime)
        {
            var states = Enum.GetValues(typeof(CharacterState)) as CharacterState[];
            foreach (var state in states) {
                animator.SetBool("state." + state.ToString(), this.state == state);
            }
        }

        public void ApplyRootMotionValues()
        {
            _movement.MoveCharacter(animator.deltaPosition);
        }

        public void ApplyRootMotionValues(float multiplier)
        {
            _movement.MoveCharacter(animator.deltaPosition * multiplier);
        }

        [CanBeNull]
        public AnimatorBodyWeight GetBodyWeight()
        {
            return BodyWeight;
        }

        public void SetBodyWeight([CanBeNull] AnimatorBodyWeight bodyWeight = null)
        {
            this.BodyWeight = bodyWeight;
        }

        public void SetLookAt(Vector3 loockAt)
        {
            this._lockAtPosition = loockAt;
        }

        // Call for calculation deltaPosition
        private void OnAnimatorMove()
        {
        }

        private void OnAnimatorIK(int layer)
        {
            if (BodyWeight != null) {
                animator.SetLookAtWeight(BodyWeight.weight, BodyWeight.bodyWeight, BodyWeight.headWeight, 0.3f);
                animator.SetLookAtPosition(_lockAtPosition);
            }
        }

        #region Events

        private void onStartAttackEvent()
        {
            events.onStartAttack?.Invoke();
        }

        private void onEndAttackEvent()
        {
            events.onEndAttack?.Invoke();
        }

        private void DrawArrow()
        {
            events.onDrawArrow?.Invoke();
        }

        private void BowShooting()
        {
            events.onBowShooting?.Invoke();
        }

        #endregion
    }
}