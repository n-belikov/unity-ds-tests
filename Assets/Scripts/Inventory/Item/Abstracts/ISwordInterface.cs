using UnityEngine;
using UnityEngine.Events;

namespace Inventory.Item.Abstracts
{
    public interface ISwordInterface : IWeaponInterface
    {
        void StartSlash();

        void EndSlash();

        void AddListener(UnityAction<GameObject> action);

        void RemoveListener(UnityAction<GameObject> action);
    }
}