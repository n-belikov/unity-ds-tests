using System.Collections;
using Inventory.Item.Abstracts;
using UnityEngine;

namespace Inventory.Item.Base
{
    public class BaseBow : MonoBehaviour, IBowInterface
    {
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");

        [SerializeField] private float _dissolveSpeed = 1f;
        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private bool _enabled = false;

        public void Enable()
        {
            _enabled = true;
            if (_renderer) {
                _renderer.material.SetFloat(Dissolve, 0f);
            }
        }

        public void Disable(bool destroy = false)
        {
            _enabled = false;
            if (destroy) {
                Destroy(gameObject);
            }
            else {
                if (_renderer && gameObject.activeInHierarchy) {
                    StartCoroutine(DissolveCoroutine());
                }
                else {
                    gameObject.SetActive(false);
                }
            }
        }

        public virtual void OnDrawArrow()
        {
            Debug.Log("Draw arrow");
        }

        public virtual void OnUnDrawArrow()
        {
            Debug.Log("Un draw arrow");
        }

        public virtual void OnShootingEvent()
        {
            Debug.Log("Shooting arrow");
        }

        private IEnumerator DissolveCoroutine()
        {
            var value = 0f;

            while (value < 1f) {
                if (_enabled) {
                    Enable();
                    yield break;
                }

                value += Time.deltaTime * _dissolveSpeed;
                _renderer.material.SetFloat(Dissolve, value);
                yield return null;
            }

            gameObject.SetActive(false);
            yield break;
        }
    }
}