using Inventory.Item.Data.Abstracts;
using UnityEngine;

namespace Inventory.Item.Data
{
    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class WeaponItemData : ItemData, IWeaponItemData
    {
        public int Attack => attack;
        public new int Limit => 1;
        public WeaponType Type => type;
        public HolderType HolderType => holderType;
        public GameObject Prefab => prefab;
        public Vector3 Position => position;
        public Vector3 Rotation => rotation;
        public string[] NormalAttackList => normalAttackList;
        public string[] HeavyAttackList => heavyAttackList;

        [SerializeField] private int attack = 0;
        [SerializeField] private WeaponType type = WeaponType.Sword;
        [SerializeField] private HolderType holderType = HolderType.LeftArm;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector3 position = Vector3.zero, rotation = Vector3.zero;
        [SerializeField] private string[] normalAttackList, heavyAttackList;
    }
}