using UnityEngine;

namespace Inventory.Item.Data.Abstracts
{
    public interface IItemData
    {
        string Title { get; }
        Sprite Icon { get; }
        int Limit { get; }
    }
}