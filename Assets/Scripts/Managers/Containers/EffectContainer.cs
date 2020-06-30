using System;
using System.Collections.Generic;
using Managers.Containers.Abstracts;
using Managers.Containers.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Containers
{
    public class EffectContainer : MonoBehaviour, IEffectContainer
    {
        public List<EffectItemData> Items => _data.Items;

        [SerializeField] private EffectContainerData _data;

        public List<EffectItemData> GetByType(EffectType type)
        {
            return _data.Items.FindAll(x => x.Type == type);
        }

        public void Create(EffectItemData item, Vector3 position)
        {
            this.Create(item, position, Quaternion.identity);
        }

        public void Create(EffectItemData item, Vector3 position, Vector3 rotation)
        {
            this.Create(item, position, Quaternion.Euler(rotation));
        }

        public void Create(EffectItemData item, Vector3 position, Quaternion rotation)
        {
            var _instance = Instantiate(item.Prefab, position, rotation);
            Destroy(_instance, 30f);
        }

        public EffectItemData GetRandomByType(EffectType type)
        {
            var items = GetByType(type);
            return items[Random.Range(0, items.Count)];
        }
    }
}