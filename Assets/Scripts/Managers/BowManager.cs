using System;
using App;
using CCamera;
using CryoDI;
using Inputs;
using Locomotion;
using Managers.Abstracts;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class BowManager : MonoBehaviour, IBowManagerInterface
    {
        [SerializeField] private CameraPosition aimingCameraPosition;
        [SerializeField] private CharacterAnimator.AnimatorBodyWeight aimingBodyWeight;

        [SerializeField] private float movementMultiplier = 0.25f, cameraForwardDistance = 5f;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] private PlayerManager playerManager;

        [Dependency] private IInputHandler InputHandler { get; set; }

        public UnityEvent OnDrawEvent => _onDrawEvent;
        public UnityEvent OnUnDrawEvent => _onUnDrawEvent;
        public UnityEvent OnShootingEvent => _onShootingEvent;

        [SerializeField, HideInInspector] private UnityEvent _onDrawEvent, _onUnDrawEvent, _onShootingEvent;

        private bool _inState = false, _canAttack = true, _isDraw = false;

        private void Awake()
        {
            GameManager.Resolve.RegisterInstance<IBowManagerInterface>(this);
        }

        public void SetCanAttack(bool value)
        {
            _canAttack = value;
        }

        public void Tick(float deltaTime)
        {
            movement.SetLookAtCamera(InputHandler.Aiming && !playerManager.IsRoll && _canAttack);

            // TODO: Добавить возможность в прыжке

            if (InputHandler.Aiming
                && movement.OnGrounded
                && _canAttack) {
                playerManager.Camera.SetCameraPosition(aimingCameraPosition);

                animator.SetState(CharacterState.Bow);
                animator.SetAiming(InputHandler.Aiming && !playerManager.IsRoll);
                animator.SetAttack(InputHandler.Attack);

                if (!_inState && !playerManager.IsRoll) {
                    playerManager.StopAnother();
                    animator.FireAnimation("BowLocomotion", true);
                    animator.SetBodyWeight(aimingBodyWeight);
                    _inState = true;
                }

                if (_inState && playerManager.IsRoll) {
                    animator.SetBodyWeight();
                    _inState = false;
                }

                animator.SetLookAt(GetLockAt());

                animator.SetMovementAxis(
                    InputHandler.Horizontal,
                    InputHandler.Vertical
                );

                // if (!movement.OnGrounded) {
                movement.SetMovementSpeed(0f);
                movement.SetMovementAxis(
                    0,
                    0
                );
                // } else {
                animator.ApplyRootMotionValues(movementMultiplier);
                // }
            }
            else {
                if (animator.GetState() == CharacterState.Bow) {
                    animator.SetState(CharacterState.Default);
                }

                if (_inState) {
                    playerManager.StopAnother();
                    animator.SetBodyWeight();

                    if (_isDraw) {
                        OnUnDrawArrowEvent();
                    }

                    _inState = false;
                }

                animator.SetAiming(false);
            }
        }


        private void OnEnable()
        {
            animator.events.onBowShooting.AddListener(OnBowShootingEvent);
            animator.events.onDrawArrow.AddListener(OnDrawArrowEvent);
            animator.events.onUnDrawArrow.AddListener(OnUnDrawArrowEvent);
        }

        private void OnDisable()
        {
            animator.events.onBowShooting.RemoveListener(OnBowShootingEvent);
            animator.events.onDrawArrow.RemoveListener(OnDrawArrowEvent);
            animator.events.onUnDrawArrow.RemoveListener(OnUnDrawArrowEvent);
        }

        #region Events

        private void OnBowShootingEvent()
        {
            _isDraw = false;
            _onShootingEvent?.Invoke();
        }

        private void OnDrawArrowEvent()
        {
            _isDraw = true;
            _onDrawEvent?.Invoke();
        }

        private void OnUnDrawArrowEvent()
        {
            _isDraw = false;
            _onUnDrawEvent?.Invoke();
        }

        #endregion

        private Vector3 GetLockAt()
        {
            Transform cameraTransform = playerManager.Camera.Transform;
            return cameraTransform.position + cameraTransform.forward * cameraForwardDistance + Vector3.down;
        }
    }
}