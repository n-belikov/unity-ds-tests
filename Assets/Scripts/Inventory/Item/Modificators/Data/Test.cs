using System;
using Inventory.Item.Data.Abstracts;
using Inventory.Item.Modificators.Abstracts;
using UnityEngine;

namespace Inventory.Item.Modificators.Data
{
    [CreateAssetMenu(fileName = "Test modificator", menuName = "Modificators", order = 0)]
    public class Test : ScriptableObject, IModificatorInterface
    {
        public int Percent => _percent;
        public Type ItemType => typeof(IWeaponItemData);

        [SerializeField] private int _percent = 0;
        
        public int Calculate(InventoryItem item, int value)
        {
            float _value = value;
            if ((item.Item as IWeaponItemData) != null) {
                _value *= 1 + (_percent / 100f);
            }

            return Mathf.RoundToInt(_value);
        }
    }
}