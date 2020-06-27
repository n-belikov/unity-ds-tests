using Inventory.Item.Data.Abstracts;
using UnityEngine;

namespace Inventory.Item.Data
{
    [CreateAssetMenu(menuName = "Items/Health")]
    public class HealthItemData : ItemData, IHealthData
    {
        public int AddHealth => addHealth;

        [SerializeField] private int addHealth = 0;
    }
}