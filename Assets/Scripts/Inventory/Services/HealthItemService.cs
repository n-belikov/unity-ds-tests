using Inventory.Item.Data.Abstracts;
using Inventory.Services.Abstracts;
using Stats.Player;
using UnityEngine;

namespace Inventory.Services
{
    public class HealthItemService : BaseItemService, IItemService
    {
        protected readonly PlayerStatsManager PlayerStatsManager;
        
        public HealthItemService(IInventoryManager inventoryManager) : base(inventoryManager)
        {
            // TODO: Change finding player stats manager
            PlayerStatsManager = GameObject.FindObjectOfType<PlayerStatsManager>();
        }

        private bool Use(IHealthData itemData)
        {
            if (PlayerStatsManager.Health == PlayerStatsManager.MaxHealth) {
                return false;
            }
            PlayerStatsManager.AddHealth(itemData.AddHealth);
            return true;
        }

        public void EquipByIndex(int index, bool equip)
        {
            if (Repository.Count <= index) return;
            var item = Repository.Get(index);
            if (!Use(item.Item as IHealthData)) return;
            item.Count--;
            if (item.Count <= 0) {
                Repository.RemoveAt(index);
                InventoryManager.InventoryUi.Show();
            }
        }
    }
}