using UnityEngine;
using System;

namespace Inventory.Item.Modificators.Abstracts
{
    public interface IModificatorInterface
    {
        int Percent { get; }
        
        Type ItemType { get; }

        int Calculate(InventoryItem item, int value);
    }
}