using App;
using Inventory.Item.Base;
using Managers.Abstracts;
using Managers.Containers.Abstracts;
using Managers.Containers.Data;
using UnityEngine;
using Xft;

namespace Inventory.Item
{
    public class Sword : BaseSword
    {
        [SerializeField] private MeleeWeaponTrail _trail;
        [SerializeField] private XWeaponTrail _xTrail;
        [SerializeField] private Transform _rayPointStart, _rayPointEnd;
        [SerializeField] private AudioSource _source;
        [SerializeField] private SoundItemData _soundSlash;


        private ISwordManagerInterface _swordManager;
        private ISoundContainer _soundContainer;

        private void Start()
        {
            _swordManager = GetComponentInParent<ISwordManagerInterface>();
            _soundContainer = GameManager.Resolve.Resolve<ISoundContainer>();
            SubscribeEvents();
            
            _xTrail?.Init();
        }

        private void SubscribeEvents()
        {
            _swordManager.OnStartSlashEvent.RemoveListener(StartSlash);
            _swordManager.OnEndSlashEvent.RemoveListener(EndSlash);

            _swordManager.OnStartSlashEvent.AddListener(StartSlash);
            _swordManager.OnEndSlashEvent.AddListener(EndSlash);
        }

        public override void StartSlash()
        {
            _soundContainer.PlayItem(_source, _soundSlash);
            base.StartSlash();
            SetTrailUse(true);
        }
        
        public override void EndSlash()
        {
            base.EndSlash();
            SetTrailUse(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isSlash) {
                InvokeDamage(other.gameObject);
            }
        }

        protected void InvokeDamage(GameObject _object)
        {
            if (_swordManager != null) {
                _swordManager.InvokeDamage(_object, GetRayPosition());
            }

            _event?.Invoke(_object);
        }

        private void SetTrailUse(bool value)
        {
            if (_trail != null) {
                _trail.Emit = value;
            }

            if (_xTrail != null) {
                if (value) {
                    _xTrail.Activate();
                } else {
                    _xTrail.Deactivate();
                }
            }
        }

        private Vector3 GetRayPosition()
        {
            if (Physics.Linecast(_rayPointStart.position, _rayPointEnd.position, out RaycastHit hit)) {
                return hit.point + hit.normal * 0.1f;
            }

            return _rayPointEnd.position;
        }
    }
}