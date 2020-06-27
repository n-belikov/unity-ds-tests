using System;
using CCamera;
using Inputs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Locomotion
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        public bool OnGrounded { get; private set; } = true;

        private CharacterController _characterController;
        private Vector3 _moveDirection;
        private Vector3 _rotateDirection;
        private Quaternion _rotation;
        private float _velocityY;

        [SerializeField]
        private Transform _camera;
        private float _horizontal, _vertical, _speed;
        public float movementSpeed = 10;
        public float rotationSpeed = 10;
        public float jumpHeight = 1f;

        [Header("Ground")]
        public float sphereCastSize = 0.5f;
        public float maxCheckGroundDistance = 1f;
        public float gravity = -12f;
        private bool _lockAtCamera = false;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _velocityY = 0f;
        }

        public void SetMovementAxis(float horizontal, float vertical)
        {
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public void SetRotationAxis(float horizontal, float vertical)
        {
            _rotateDirection = _camera.forward * vertical + _camera.right * horizontal;
            _rotateDirection.y = 0f;
            
            if (_rotateDirection.normalized != Vector3.zero)
                _rotation = Quaternion.LookRotation(_rotateDirection.normalized, transform.up);
        }

        public void SetMovementSpeed(float speed)
        {
            _speed = speed;
        }

        public void MoveCharacter(Vector3 move)
        {
            _characterController.Move(move);
        }

        public void SimpleMoveCharacter(Vector3 move)
        {
            _characterController.SimpleMove(move);
        }

        public void Tick(float deltaTime)
        {
            CalculateGravity(deltaTime);
            Move(deltaTime);
            RotateDirection(deltaTime);
        }
        
        public void FixedTick(float deltaTime)
        {
            RaycastHit hit;
            if (_characterController.isGrounded) {
                OnGrounded = true;
                return;
            }
            if (Physics.SphereCast(transform.position + transform.up, sphereCastSize, Vector3.down, out hit, maxCheckGroundDistance)) {
                Debug.DrawLine(transform.position, hit.point, Color.red, 0.1f);
                OnGrounded = true;
            } else {
                OnGrounded = false;
            }
        }

        private void RotateDirection(float deltaTime)
        {
            _rotateDirection = _camera.forward * _vertical + _camera.right * _horizontal;
            _rotateDirection.y = 0f;
            
            if (_rotateDirection.normalized != Vector3.zero)
                _rotation = Quaternion.LookRotation(_rotateDirection.normalized, transform.up);

            Vector3 eulerAngles = transform.eulerAngles;
            float differenceRotation = _rotation.eulerAngles.y - eulerAngles.y;
            float eulerY = eulerAngles.y;
            if (Mathf.Abs(differenceRotation) > 0) eulerY = _rotation.eulerAngles.y;
            Vector3 euler = new Vector3(0, eulerY, 0);

            if (_lockAtCamera) { 
                Vector3 cameraForward = _camera.TransformDirection(Vector3.forward);
                cameraForward.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, _rotation = Quaternion.LookRotation(cameraForward),
                    rotationSpeed * Time.deltaTime);
            } else {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler),
                    rotationSpeed * deltaTime);
            }
        }

        private void CalculateGravity(float deltaTime)
        {
            if (!_characterController.isGrounded) {
                _velocityY += deltaTime * gravity;
            } else {
                if (_velocityY < 0) {
                    _velocityY = 0;
                }
            }
        }
        
        public void Jump()
        {
            float jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight); 
            _velocityY = jumpVelocity;
        }

        public void SetLookAtCamera(bool value)
        {
            _lockAtCamera = value;
        }

        private void Move(float delta)
        {
            if (!_lockAtCamera) {
                _moveDirection = transform.forward * _speed;
            } else {
                Vector3 forward = _camera.TransformDirection(Vector3.forward),
                        right = _camera.TransformDirection(Vector3.right); 
                _moveDirection = forward * _vertical + right * _horizontal;
            }

            _moveDirection.y = 0f;
            _moveDirection *= movementSpeed;
            _characterController.Move((_moveDirection + Vector3.up * _velocityY) * delta);
        }
    }
}