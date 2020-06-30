using System;
using App;
using CryoDI;
using DG.Tweening;
using Inputs;
using Inventory.Item.Data.Abstracts;
using Locomotion;
using Managers.Abstracts;
using Managers.Containers.Abstracts;
using Stats.Abstracts;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    [RequireComponent(
        typeof(PlayerManager), 
        typeof(CharacterAnimator),
        typeof(CharacterMovement)
        )]
    public class SwordManager : MonoBehaviour, ISwordManagerInterface
    {
        public enum SwordState
        {
            Default, Attack, Heavy
        }
        
        [SerializeField] private float attackMultiplier = 1f,
            heavyAttackMultiplier = 1f,
            delayToNextAttack = 0.15f,
            delayReady = 5f;

        [SerializeField]
        private string[] attackList = new string[] { };
        
        public UnityEvent OnStartSlashEvent => _onStartSlashEvent;
        public UnityEvent OnEndSlashEvent => _onEndSlashEvent;

        public bool IsAttack => _state != SwordState.Default;

        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] private PlayerManager playerManager;

        [Dependency] private IInputHandler InputHandler { get; set; }
        [Dependency] private IEffectContainer EffectContainer { get; set; }

        private bool _canAttack = true;
        private SwordState _state = SwordState.Default;
        private bool _availableNextAnimation;
        private int _currentAttackListIndex;
        private float _delayToNextAttackTimer, _delayReadyTimer;

        [SerializeField, HideInInspector] private UnityEvent _onStartSlashEvent, _onEndSlashEvent;

        private void Start()
        {
            GameManager.Resolve.RegisterInstance<ISwordManagerInterface>(this);
            movement = GetComponent<CharacterMovement>();
            animator = GetComponentInChildren<CharacterAnimator>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void OnEnable()
        {
            animator.events.onStartAttack.AddListener(StartSlashEvent);
            animator.events.onEndAttack.AddListener(EndSlashEvent);
        }

        private void OnDisable()
        {
            animator.events.onStartAttack.RemoveListener(StartSlashEvent);
            animator.events.onEndAttack.RemoveListener(EndSlashEvent);
        }

        public void SetCanAttack(bool canAttack)
        {
            _canAttack = canAttack;
        }

        public void Tick(float deltaTime)
        {
            if (IsAttack) {
                movement.SetMovementSpeed(0f);
                movement.SetMovementAxis(0f, 0f);

                animator.ApplyRootMotionValues(_state == SwordState.Heavy ? heavyAttackMultiplier : attackMultiplier);
            }

            if (!_canAttack && animator.GetState() == CharacterState.Sword) {
                animator.SetState(CharacterState.Default);
            }

            if (!IsAttack) {
                if (_delayToNextAttackTimer > 0) {
                    _delayToNextAttackTimer -= Time.deltaTime;
                    if (_delayToNextAttackTimer <= 0) {
                        _currentAttackListIndex = 0;
                    }
                }

                if (_delayReadyTimer > 0) {
                    _delayReadyTimer -= Time.deltaTime;
                    if (_delayReadyTimer <= 0) {
                        animator.SetState(CharacterState.Default);
                    }
                }
            }

            if (animator.GetState() == CharacterState.Sword) {
                SetMovementAxis(deltaTime);
            }

            if (InputHandler.Attack && !InputHandler.Aiming && !playerManager.IsJumping && _canAttack) {
                if (!IsAttack && InputHandler.Sprint) {
                    StartAttack(true);
                }
                else {
                    if (!IsAttack || _availableNextAnimation) {
                        StartAttack(false);
                    }
                }
            }
        }

        public void InvokeDamage(GameObject gameObject, Vector3 hitPoint)
        {
            var damagable = gameObject.GetComponent<IDamagable>();
            if (!ReferenceEquals(damagable, playerManager)) {
                var damage = playerManager.Stats.GetDamage(WeaponType.Sword);

                var effect = EffectContainer.GetRandomByType(EffectType.Hit);
                EffectContainer.Create(effect, hitPoint);
                
                damagable.TakeDamage(damage, playerManager);
            }
        }

        public void InvokeDamage(GameObject gameObject, Transform hitTransform)
        {
            InvokeDamage(gameObject, hitTransform.position);
        }

        public void StopAttack()
        {
            _state = SwordState.Default;
        }

        private void StartAttack(bool isHeavy)
        {
            playerManager.StopAnother();
            _availableNextAnimation = false;

            _state = isHeavy ? SwordState.Heavy : SwordState.Attack;
            
            animator.FireAnimation(isHeavy ? "Attack_Heavy" : GetCurrentAnimation(), true);

            animator.SetState(CharacterState.Sword);
        }

        private void SetMovementAxis(float deltaTime)
        {
            var speed = InputHandler.Speed;
            speed *= InputHandler.Sprint ? 1f : 0.5f;
            animator.SetMovementAxis(
                0,
                speed);
        }

        private void StartSlashEvent()
        {
            _onStartSlashEvent?.Invoke();
        }

        private void EndSlashEvent()
        {
            NextAttackAnimation();
            StopAttack();
            animator.SetState(CharacterState.Sword);
            _delayReadyTimer = delayReady;
            _delayToNextAttackTimer = delayToNextAttack;

            _onEndSlashEvent?.Invoke();
        }

        private string GetCurrentAnimation()
        {
            return attackList[_currentAttackListIndex];
        }

        private void NextAttackAnimation()
        {
            _currentAttackListIndex++;
            if (_currentAttackListIndex >= attackList.Length) {
                _currentAttackListIndex = 0;
            }
        }
    }
}