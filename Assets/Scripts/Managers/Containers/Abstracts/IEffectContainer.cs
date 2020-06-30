using UnityEngine;
using System.Collections.Generic;
using Managers.Containers.Data;

namespace Managers.Containers.Abstracts
{
    public enum EffectType
    {
        Hit, Blood, PickUp
    }
    
    public interface IEffectContainer
    {
        List<EffectItemData> Items { get; }

        List<EffectItemData> GetByType(EffectType type);

        void Create(EffectItemData item, Vector3 position);

        void Create(EffectItemData item, Vector3 position, Vector3 rotation);

        void Create(EffectItemData item, Vector3 position, Quaternion rotation);

        EffectItemData GetRandomByType(EffectType type);
    }
}