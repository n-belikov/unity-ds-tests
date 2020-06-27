using System.Collections.Generic;
using Inventory.Item.Data;
using Inventory.Item.Data.Abstracts;
using Inventory.Item.Modificators.Abstracts;
using Inventory.Item.Modificators.Data;
using UnityEngine;

namespace Inventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public IItemData Item => item;
        public bool equip = false;
        
        public int Count
        {
            get => _count;
            set => _count = Mathf.Min(Item.Limit, value);
        }

        [SerializeField] private ItemData item;
        [SerializeField] private int _count = 1;
    }
}