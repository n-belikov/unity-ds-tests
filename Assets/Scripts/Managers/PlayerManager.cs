using CCamera;
using CryoDI;
using Inputs;
using Inventory;
using Inventory.Item.Data.Abstracts;
using UnityEngine;
using Locomotion;
using Stats.Abstracts;

namespace Managers
{
    public enum PlayerState
    {
        Roll, Jump, Default
    }

    public class PlayerManager : MonoBehaviour, IDamagable
    {
        [SerializeField] private float rollMultiplier = 1f;
        [SerializeField] [Range(0f, 1f)] private float rollStart = 0.3f;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] private float _jumpRollForward = 10f;
        [Dependency] private IInventoryManager InventoryManager { get; set; }
        [Dependency] private IInputHandler InputHandler { get; set; }
        [Dependency] public ICamera Camera { get; private set; }
        [Dependency] public IPlayerStats Stats { get; private set; }
        
        public bool IsJumping => _state == PlayerState.Jump;
        public bool IsRoll => _state == PlayerState.Roll;
        public PlayerState State => _state;
        public bool DisableGravity { get; set; } = false;

        private SwordManager _swordManager;
        private BowManager _bowManager;
        private bool _inMenu = false;
        private PlayerState _state = PlayerState.Default;
        private CharacterState _previousState = CharacterState.Default;

        private void Start()
        {
            animator = GetComponentInChildren<CharacterAnimator>();
            movement = GetComponent<CharacterMovement>();

            // not required
            _swordManager = GetComponent<SwordManager>();
            _bowManager = GetComponent<BowManager>();
        }

        public void SetInMenu(bool value)
        {
            _inMenu = value;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            InputHandler.Tick(deltaTime);
            Camera.ResetValues();

            Move(deltaTime);
            Jumping(deltaTime);
            Rolling(deltaTime);


            if (_bowManager) {
                var canBowAttack = InventoryManager == null || InventoryManager.HasEquipWeapon(WeaponType.Bow);
                _bowManager.SetCanAttack(canBowAttack && !_inMenu);
                _bowManager.Tick(deltaTime);
            }

            if (_swordManager) {
                var canSwordAttack = InventoryManager == null || InventoryManager.HasEquipWeapon(WeaponType.Sword);
                _swordManager.SetCanAttack(canSwordAttack && !_inMenu);
                _swordManager.Tick(deltaTime);
            }

            UpdateMenuValuesAsMenu(deltaTime);
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            UpdateWeaponsInHands(deltaTime);

            // Complete previous actions in movement
            movement.Tick(deltaTime);

            // Complete previous actions in camera
            Camera.Tick(deltaTime);

            animator.StateTick(deltaTime);
            animator.MoveTick(deltaTime);
        }

        private void UpdateWeaponsInHands(float deltaTime)
        {
            if (_previousState != animator.GetState()) {
                if (InventoryManager != null) {
                    InventoryManager.SetWeaponVisible(WeaponType.Bow, animator.GetState() == CharacterState.Bow);
                    InventoryManager.SetWeaponVisible(WeaponType.Sword, animator.GetState() == CharacterState.Sword);
                }

                _previousState = animator.GetState();
            }
        }

        private void UpdateMenuValuesAsMenu(float deltaTime)
        {
            var multiplier = _inMenu ? 0f : 1f;

            Camera.SetMouseAxis(
                multiplier * InputHandler.MouseX,
                multiplier * InputHandler.MouseY
            );

            if (_inMenu) {
                movement.SetMovementAxis(0, 0);
                movement.SetMovementSpeed(0);

                animator.SetMovementAxis(0, 0);
                animator.SetMotionValue(0);
            }
        }

        private void Move(float deltaTime)
        {
            float speedMultiplier = InputHandler.Sprint ? 1f : 0.5f;

            movement.SetMovementSpeed(InputHandler.Speed * speedMultiplier);
            movement.SetMovementAxis(InputHandler.Horizontal, InputHandler.Vertical);

            animator.SetMotionValue(InputHandler.Speed * speedMultiplier);
            animator.SetMovementAxis(InputHandler.Horizontal, InputHandler.Vertical);
        }

        private void Rolling(float deltaTime)
        {
            if (_state == PlayerState.Roll) {
                movement.SetMovementSpeed(0f);
                movement.SetMovementAxis(0f, 0f);
                animator.ApplyRootMotionValues(rollMultiplier);
            }

            if (InputHandler.Roll && movement.OnGrounded && _state == PlayerState.Default) {
                StopAnother();
                movement.SetRotationAxis(InputHandler.Horizontal, InputHandler.Vertical);
                animator.FireAnimation("StandToRoll", true, rollStart);
                _state = PlayerState.Roll;
            }
        }

        private Vector3 _jumpRollDirection = Vector3.zero;

        private void Jumping(float deltaTime)
        {
            bool onGrounded = movement.OnGrounded;

            if (_state == PlayerState.Jump && onGrounded) {
                movement.SetMovementSpeed(0f);
                movement.SetMovementAxis(0f, 0f);
                animator.ApplyRootMotionValues(0.25f);
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_jumpRollDirection != Vector3.zero)
                    _jumpRollDirection = Vector3.zero;
            }

            if (!onGrounded && _state == PlayerState.Jump) {
                if (InputHandler.Roll && _jumpRollDirection == Vector3.zero) {
                    _jumpRollDirection = transform.forward * _jumpRollForward;
                }
                if (_jumpRollDirection != Vector3.zero) {
                    _jumpRollDirection = Vector3.Lerp(_jumpRollDirection, Vector3.zero, deltaTime);
                    movement.MoveCharacter(_jumpRollDirection * deltaTime);
                }
            }

            animator.SetGrounded(onGrounded);
            animator.SetIsJumping(_state == PlayerState.Jump);
            if (!onGrounded && _state != PlayerState.Jump && !DisableGravity) {
                StopAnother();
                _state = PlayerState.Jump;
                animator.FireAnimation("Falling", true);
            }

            if (InputHandler.Jump && movement.OnGrounded && _state == PlayerState.Default) {
                StopAnother();
                movement.Jump();
                animator.FireAnimation("JumpStart", true);
            }
        }

        public void StopAnother()
        {
            _swordManager.StopAttack();
            _state = PlayerState.Default;
        }

        public void OnJumpEventListener(bool value)
        {
            UpdateState(PlayerState.Jump, value);
        }

        public void OnRollEventListener(bool value)
        {
            UpdateState(PlayerState.Roll, value);
        }

        private void UpdateState(PlayerState state, bool value)
        {
            if (value) {
                _state = state;
            }
            else if (_state == state) {
                _state = PlayerState.Default;
            }
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.deltaTime;
            movement.FixedTick(deltaTime);
        }

        public void TakeDamage(int value, IDamagable from)
        {
            Debug.Log("I Take Damage");
        }
    }
}