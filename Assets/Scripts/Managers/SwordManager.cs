using System;
using App;
using CryoDI;
using DG.Tweening;
using Inputs;
using Inventory.Item.Data.Abstracts;
using Locomotion;
using Managers.Abstracts;
using Stats.Abstracts;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class SwordManager : MonoBehaviour, ISwordManagerInterface
    {
        [SerializeField] private float attackMultiplier = 1f,
            heavyAttackMultiplier = 1f,
            delayToNextAttack = 0.15f,
            delayReady = 5f;

        public string[] attackList = new string[] { };
        
        public UnityEvent OnStartSlashEvent => _onStartSlashEvent;
        public UnityEvent OnEndSlashEvent => _onEndSlashEvent;

        public bool IsAttack => _isAttack;

        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] private PlayerManager playerManager;

        [Dependency] private IInputHandler InputHandler { get; set; }

        private bool _canAttack = true;

        private bool _isAttack = false;
        private bool _isHeavy = false;
        private bool _availableNextAnimation = false;

        private int _currentAttackListIndex = 0;

        private float _delayToNextAttackTimer = 0, _delayReadyTimer = 0;

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
            if (_isAttack) {
                movement.SetMovementSpeed(0f);
                movement.SetMovementAxis(0f, 0f);

                animator.ApplyRootMotionValues(_isHeavy ? heavyAttackMultiplier : attackMultiplier);
            }

            if (!_canAttack && animator.GetState() == CharacterState.Sword) {
                animator.SetState(CharacterState.Default);
            }

            if (!_isAttack) {
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
                if (!_isAttack && InputHandler.Sprint) {
                    StartAttack(true);
                }
                else {
                    if (!_isAttack || _availableNextAnimation) {
                        StartAttack(false);
                    }
                }
            }
        }

        private void StartAttack(bool isHeavy)
        {
            playerManager.StopAnother();
            _availableNextAnimation = false;
            _isAttack = true;
            _isHeavy = isHeavy;
            animator.FireAnimation(isHeavy ? "Attack_Heavy" : GetCurrentAnimation(), true);

            animator.SetState(CharacterState.Sword);
        }

        private void SetMovementAxis(float deltaTime)
        {
            float speed = InputHandler.Speed;
            speed *= InputHandler.Sprint ? 1f : 0.5f;
            animator.SetMovementAxis(
                0,
                speed);
        }

        public void InvokeDamage(GameObject gameObject)
        {
            var damagable = gameObject.GetComponent<IDamagable>();
            if (damagable != null) {
                var damage = playerManager.Stats.GetDamage(WeaponType.Sword);
                
                damagable.TakeDamage(damage, playerManager);
            }
        }

        private void StartSlashEvent()
        {
            _onStartSlashEvent?.Invoke();
        }

        private void EndSlashEvent()
        {
            NextAttackAnimation();
            _isAttack = false;
            _isHeavy = false;
            animator.SetState(CharacterState.Sword);
            _delayReadyTimer = delayReady;
            _delayToNextAttackTimer = delayToNextAttack;

            _onEndSlashEvent?.Invoke();
        }

        private string GetCurrentAnimation()
        {
            return attackList[_currentAttackListIndex];
        }

        public void StopAttack()
        {
            _isAttack = false;
            _isHeavy = false;
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