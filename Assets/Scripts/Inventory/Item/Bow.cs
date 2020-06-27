using System;
using App;
using Inventory.Item.Abstracts;
using Inventory.Item.Base;
using Managers.Abstracts;
using UnityEngine;

namespace Inventory.Item
{
    public class Bow : BaseBow
    {
        private IBowManagerInterface _bowManager;

        [SerializeField] private Transform _throwable;
        [SerializeField] private BaseArrow _arrow;
        private Transform _current;

        private void Start()
        {
            _bowManager = GetComponentInParent<IBowManagerInterface>();

            SubscribeEvents();
        }

        public override void OnDrawArrow()
        {
            if (_current) {
                Destroy(_current.gameObject);
            }

            var arrow = Instantiate(_arrow, _throwable);
            arrow.gameObject.name = "Arrow";
            _current = arrow.transform;

            _current.localPosition = Vector3.zero;
            _current.localRotation = Quaternion.identity;
        }

        public override void OnUnDrawArrow()
        {
            if (_current) {
                Destroy(_current.gameObject);
            }
        }

        public override void OnShootingEvent()
        {
            var arrow = _current.GetComponent<IArrowInterface>();
            arrow?.Shot();
        }

        private void SubscribeEvents()
        {
            _bowManager.OnDrawEvent.RemoveListener(OnDrawArrow);
            _bowManager.OnUnDrawEvent.RemoveListener(OnUnDrawArrow);
            _bowManager.OnShootingEvent.RemoveListener(OnShootingEvent);

            _bowManager.OnDrawEvent.AddListener(OnDrawArrow);
            _bowManager.OnUnDrawEvent.AddListener(OnUnDrawArrow);
            _bowManager.OnShootingEvent.AddListener(OnShootingEvent);
        }
    }
}