using Inventory.Item.Data.Abstracts;

namespace Stats.Abstracts
{
    public interface IPlayerStats
    { 
        int Health { get; }

        int GetDamage(WeaponType type);
    }
}