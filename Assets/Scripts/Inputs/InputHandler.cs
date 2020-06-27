using System;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputHandler : MonoBehaviour, IInputHandler
    {
        public float Speed { get; private set; } = 0f;
        public float Horizontal { get; private set; } = 0f;
        public float Vertical { get; private set; } = 0f;

        public float MouseX { get; private set; } = 0f;
        public float MouseY { get; private set; } = 0f;

        public bool Jump { get; private set; } = false;

        public bool Roll { get; private set; } = false;

        public bool Sprint { get; private set; } = false;

        public bool Attack { get; private set; } = false;

        public bool Aiming { get; private set; } = false;

        public bool Menu { get; private set; } = false;

        public bool Inventory { get; private set; } = false;
        
        private PlayerControls _controls;
        private Vector2 _cameraAxis;
        private Vector2 _movementAxis;
        private bool _movable;

        [SerializeField]
        private bool fixedAiming = false;

        private void OnEnable()
        {
            if (_controls == null) {
                _controls = new PlayerControls();
                _controls.PlayerMovement.Movement.performed += _controls => _movementAxis = _controls.ReadValue<Vector2>();
                _controls.PlayerMovement.Camera.performed += _controls => _cameraAxis = _controls.ReadValue<Vector2>();
            }
            _controls.Enable();
        }

        public void Tick(float deltaTime)
        {
            MovementTick(deltaTime);
            ActionsTick(deltaTime);
            MenuTick(deltaTime);
            MouseTick(deltaTime);
        }

        private void MouseTick(float deltaTime)
        {
            MouseX = _cameraAxis.x;
            MouseY = _cameraAxis.y;
        }

        private void MenuTick(float deltaTime)
        {
            Menu = _controls.MenuActions.Menu.phase == InputActionPhase.Started;
            Inventory = _controls.MenuActions.Inventory.phase == InputActionPhase.Started;
        }

        private void ActionsTick(float deltaTime)
        {
            Jump = _controls.PlayerActions.Jump.phase == InputActionPhase.Started;
            Sprint = _controls.PlayerActions.Sprint.phase == InputActionPhase.Started;
            Roll = _controls.PlayerActions.Roll.phase == InputActionPhase.Started;
            Attack = _controls.PlayerActions.Attack.phase == InputActionPhase.Started;
            Aiming = _controls.PlayerActions.Aiming.phase == InputActionPhase.Started || fixedAiming;
        }

        private void MovementTick(float deltaTime)
        {
            Horizontal = _movementAxis.x;
            Vertical = _movementAxis.y;
            Speed = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
        }

        private void OnDisable()
        {
            _controls.Disable();
        }
    }

    public interface IInputHandler
    {
        float Speed { get; }
        float Horizontal { get; }
        float Vertical { get; }
        float MouseX { get; }
        float MouseY { get; }
        bool Jump { get; }
        bool Roll { get; }
        bool Sprint { get; }
        bool Attack { get; }
        bool Aiming { get; }
        bool Menu { get; }
        bool Inventory { get; }
        
        void Tick(float deltaTime);
    }
}
