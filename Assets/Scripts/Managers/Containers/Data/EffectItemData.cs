using System.Collections.Generic;
using Managers.Containers.Abstracts;
using UnityEngine;

namespace Managers.Containers.Data
{
    [CreateAssetMenu(fileName = "Item", menuName = "Effects/Item")]
    public class EffectItemData : ScriptableObject
    {
        public EffectType Type => _type;
        public GameObject Prefab => _prefab;
        
        [SerializeField] private GameObject _prefab;
        [SerializeField] private EffectType _type = EffectType.Hit;
    }

    [CreateAssetMenu(fileName = "Container", menuName = "Effects/Container")]
    public class EffectContainerData : ScriptableObject
    {
        public List<EffectItemData> Items = new List<EffectItemData>();
    }
}