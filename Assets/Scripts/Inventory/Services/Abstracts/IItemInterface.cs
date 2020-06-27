namespace Inventory.Services.Abstracts
{
    public interface IItemService
    {
        void EquipByIndex(int index, bool equip);
    }
}