using System;
using System.Runtime.CompilerServices;
using Inventory.Item.Abstracts;
using Inventory.Item.Base;
using Managers.Abstracts;
using UnityEngine;

namespace Inventory.Item
{
    public class Sword : BaseSword
    {
        [SerializeField] private MeleeWeaponTrail _trail;
        private ISwordManagerInterface _swordManager;
        
        private void Start()
        {
            _swordManager = GetComponentInParent<ISwordManagerInterface>();

            SubscribeEvents();

            _trail?.SetEmit(false);
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
            base.StartSlash();
            _trail?.SetEmit(true);
        }

        public override void EndSlash()
        {
            base.EndSlash();
            _trail?.SetEmit(false);
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
                _swordManager.InvokeDamage(_object);
            }

            _event?.Invoke(_object);
        }
    }
}