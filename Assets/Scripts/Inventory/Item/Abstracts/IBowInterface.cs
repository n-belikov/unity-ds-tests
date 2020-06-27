namespace Inventory.Item.Abstracts
{
    public interface IBowInterface : IWeaponInterface
    {
        void OnDrawArrow();

        void OnUnDrawArrow();

        void OnShootingEvent();
    }
}