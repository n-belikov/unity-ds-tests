using UnityEngine;
using UnityEngine.Serialization;

namespace CCamera
{
    // TODO: Refactor this
    public class CameraController : MonoBehaviour, ICamera
    {
        public Transform Transform => transform;

        [SerializeField] public float speedOffsetChange = 35.0f;
        [SerializeField] private CameraPosition cameraPosition = new CameraPosition();

        [SerializeField] private float mouseXSens = 125.0f, mouseYSens = 5.0f;
        [SerializeField] private float vLookAngleThreshold = 80.0f;

        [SerializeField] private float smoothDampValue = 1f;

        [SerializeField] private Transform lockAtTarget;
        [SerializeField] private Transform moveAtTarget;

        private bool _move = true;

        private float _mouseX = 0, _mouseY = 0;
        private float _x = 0.0f, _y = 0.0f;
        private float _vLookAngle = 0;

        private Quaternion _rotation;
        private Vector3 _position;
        private Vector3 _cameraRotationPointCurrent;
        private Vector3 _cameraPositionOffsetCurrent;
        private Vector3 _smoothDampVelocity;

        private CameraPosition _cameraPosition;

        public void SetLockAt(Transform target)
        {
            lockAtTarget = target;
        }

        public void SetMoveAt(Transform target)
        {
            moveAtTarget = target;
        }


        public void ResetValues()
        {
            _cameraPosition = cameraPosition;
            _move = true;
        }

        public void SetMouseAxis(float x, float y)
        {
            _mouseX = x;
            _mouseY = y;
        }

        public void SetCameraPosition(CameraPosition currentCameraPosition)
        {
            _cameraPosition = currentCameraPosition;
        }

        public void Tick(float deltaTime)
        {
            _vLookAngle -= _mouseY * mouseYSens;
            _vLookAngle = ClampAngle(_vLookAngle, -vLookAngleThreshold, vLookAngleThreshold);

            _x += _mouseX * mouseXSens * 0.02f;
            _y -= _mouseY * mouseYSens;
            _y = _vLookAngle;

            _cameraRotationPointCurrent = Vector3.MoveTowards(
                _cameraRotationPointCurrent, _cameraPosition.cameraRotationPointNormal,
                deltaTime * speedOffsetChange
            );

            _cameraPositionOffsetCurrent = Vector3.MoveTowards(
                _cameraPositionOffsetCurrent, _cameraPosition.cameraPositionOffsetNormal,
                deltaTime * speedOffsetChange
            );

            _rotation = Quaternion.Euler(_y, _x, 0);
            _position = CalculateCameraPosition(_cameraRotationPointCurrent, _cameraPositionOffsetCurrent, _rotation);

            transform.position = SmoothDamp(_position, deltaTime);
            transform.rotation = _rotation;
        }

        private Vector3 SmoothDamp(Vector3 nextPosition, float deltaTime)
        {
            return Vector3.SmoothDamp(transform.position, nextPosition, ref _smoothDampVelocity,
                deltaTime / smoothDampValue);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) {
                angle += 360f;
            }

            if (angle > 360f) {
                angle -= 360f;
            }

            return Mathf.Clamp(angle, min, max);
        }

        private Vector3 CalculateCameraPosition(Vector3 rotPoint, Vector3 posOffset, Quaternion rotation)
        {
            return moveAtTarget.position + rotation * new Vector3(posOffset.x, posOffset.y, -posOffset.z) +
                   new Vector3(rotPoint.x, rotPoint.y, rotPoint.z);
        }
    }

    public interface ICamera
    {
        Transform Transform { get; }

        void ResetValues();

        void SetMouseAxis(float x, float y);

        void SetCameraPosition(CameraPosition currentCameraPosition);

        void Tick(float deltaTime);
    }


    [System.Serializable]
    public class CameraPosition
    {
        public Vector3 cameraRotationPointNormal;
        public Vector3 cameraRotationPointCrouching;

        public Vector3 cameraPositionOffsetNormal;
        public Vector3 cameraPositionOffsetCrouching;

        public CameraPosition()
        {
            cameraRotationPointNormal = new Vector3(0, 1.3f, 0);
            cameraRotationPointCrouching = new Vector3(0, 1.1f, 0);

            cameraPositionOffsetNormal = new Vector3(0.7f, 0.33f, 3.19f);
            cameraPositionOffsetCrouching = new Vector3(0.36f, 0f, 0.3f);
        }
    }
}