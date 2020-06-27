using System.Collections.Generic;
using System.Linq;
using Inventory.Item.Data;
using Inventory.Services.Abstracts;

namespace Inventory.Services
{
    public class WeaponItemService : BaseItemService, IItemService
    {
        public WeaponItemService(IInventoryManager inventoryManager) : base(inventoryManager)
        {
        }

        public void EquipByIndex(int index, bool equip)
        {
            if (Repository.Count <= index) return;
            var current = Repository.Get(index);
            var weapon = current.Item as WeaponItemData;
            if (weapon == null) {
                return;
            }

            var equipList = Repository.GetEquipItemsByType(
                typeof(WeaponItemData)
            ).Where(x => ((WeaponItemData) x.Item).Type == weapon.Type);

            foreach (var inventoryItem in equipList) {
                inventoryItem.equip = false;
            }

            current.equip = equip;

            if (current.equip) {
                InventoryManager.InitWeaponItem(weapon.HolderType, weapon, false);
            }
            else {
                InventoryManager.DequipWeaponItem(weapon.HolderType);
            }
        }
    }
}