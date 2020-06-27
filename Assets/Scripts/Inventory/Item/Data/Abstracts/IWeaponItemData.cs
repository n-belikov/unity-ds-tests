using UnityEngine;

namespace Inventory.Item.Data.Abstracts
{
    public enum WeaponType
    {
        Sword, Bow
    }
    public interface IWeaponItemData
    {
        int Attack { get; }
        WeaponType Type { get; }
        HolderType HolderType { get; }
        GameObject Prefab { get; }
        Vector3 Position { get; }
        Vector3 Rotation { get; }

        string[] NormalAttackList { get; }
        string[] HeavyAttackList { get; }
    }
}