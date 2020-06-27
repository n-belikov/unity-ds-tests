namespace Inventory.Item.Abstracts
{
    public interface IWeaponInterface
    {
        void Enable();

        void Disable(bool destroy = false);
    }
}