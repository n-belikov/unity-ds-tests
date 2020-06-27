using Inventory.Item.Data.Abstracts;
using UnityEngine;

namespace Inventory.Item.Data
{
    [CreateAssetMenu(menuName = "Items/Default")]
    public class ItemData : ScriptableObject, IItemData
    {
        public string Title => title;
        public Sprite Icon => icon;
        public int Limit => (int) limit;

        [SerializeField] protected string title;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected uint limit = 1;
    }
}