using Inventory.Repository;
using Inventory.Services.Abstracts;

namespace Inventory.Services
{
    public abstract class BaseItemService
    {
        protected readonly IInventoryManager InventoryManager;
        protected readonly IInventoryRepository Repository;

        protected BaseItemService(IInventoryManager inventoryManager)
        {
            InventoryManager = inventoryManager;
            Repository = InventoryManager.Repository;
        }
    }
}