using System;
using Stats.Abstracts;
using UnityEngine;

namespace Inventory.Item.Abstracts
{
    public class BaseArrow : MonoBehaviour, IArrowInterface
    {
        protected Rigidbody _rigidbody;

        [SerializeField] private float _force = 10f;
        private IDamagable _disableDamageObject;
        private bool _isShooting = false;

        private void Start()
        {
            _disableDamageObject = GetComponentInParent<IDamagable>();
        }

        private void Update()
        {
            if (_rigidbody) {
                RotateByCenterOfMass();
            }
        }

        protected virtual void RotateByCenterOfMass()
        {
            _rigidbody.centerOfMass = transform.forward;
            if (_rigidbody.velocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!_isShooting) {
                return;
            }
            var damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable == null || !damagable.Equals(_disableDamageObject)) {
                if (_rigidbody) {
                    Destroy(_rigidbody);
                }
            }
        }

        public virtual void Shot()
        {
            _isShooting = true;
            transform.parent = null;
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            if (!_rigidbody) {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            _rigidbody.AddForce(transform.forward * _force, ForceMode.VelocityChange);
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            Destroy(gameObject, 20f);
        }
    }

    public interface IArrowInterface
    {
        void Shot();
    }
}