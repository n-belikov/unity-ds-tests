using Inventory.Item.Abstracts;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using System.Collections;

namespace Inventory.Item.Base
{
    public abstract class BaseSword :  MonoBehaviour, ISwordInterface
    { 
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");

        [SerializeField] private float _dissolveSpeed = 1f;
        [SerializeField] private MeshRenderer _renderer;
        
        protected GameObjectEvent _event;
        protected bool _isSlash;
        private Coroutine _currentCoroutine;

        public virtual void StartSlash()
        {
            _isSlash = true;
        }

        public virtual void EndSlash()
        {
            _isSlash = false;
        }

        public void AddListener(UnityAction<GameObject> action)
        {
            _event.AddListener(action);
        }

        public void RemoveListener(UnityAction<GameObject> action)
        {
            _event.RemoveListener(action);
        }

        public void Enable()
        {
            if (_currentCoroutine != null) {
                StopCoroutine(_currentCoroutine);
            }
            if (_renderer) {
                _renderer.material.SetFloat(Dissolve, 0f);
            }
        }

        private IEnumerator DissolveCoroutine()
        {
            var value = 0f;

            while (value < 1f) {
                value += Time.deltaTime * _dissolveSpeed;
                _renderer.material.SetFloat(Dissolve, value);
                yield return null;
            }
            gameObject.SetActive(false);
        }
        
        public void Disable(bool destroy = false)
        {
            if (destroy) {
                Destroy(gameObject);
            } else {
                if (_renderer && gameObject.activeInHierarchy) {
                    _currentCoroutine = StartCoroutine(DissolveCoroutine());
                } else {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}