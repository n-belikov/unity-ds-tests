using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Item.Abstracts;
using Inventory.Item.Data.Abstracts;
using Inventory.Repository;
using Inventory.Services;
using Inventory.Services.Abstracts;
using UI;
using UI.Abstracts;
using UnityEngine;

namespace Inventory
{
    public enum HolderType
    {
        LeftArm, RightArm
    }

    public class InventoryManager : MenuPage, IInventoryManager
    {
        public IInventoryRepository Repository => _repository;
        public InventoryUI InventoryUi => inventoryUi;

        private readonly Dictionary<Type, IItemService> _itemServices = new Dictionary<Type, IItemService>();

        [SerializeField] private GameMenuManager _gameMenuManager;
        [SerializeField] private InventoryUI inventoryUi;
        [SerializeField] private List<Holder> holders = new List<Holder>();
        [SerializeField] private InventoryRepository _repository = new InventoryRepository();

        private void Awake()
        {
            _itemServices.Add(typeof(IWeaponItemData), new WeaponItemService(this));
            _itemServices.Add(typeof(IHealthData), new HealthItemService(this));
        }

        private void Start()
        {
            foreach (var item in Repository.All()) {
                if (item.equip && item.Item is IWeaponItemData weapon) {
                    InitWeaponItem(weapon.HolderType, weapon, false);
                }
            }

            inventoryUi.Init(this);
        }
    
        public bool HasEquipWeapon(WeaponType type)
        {
            return GetEquipWeapon(type) != null;
        }

        public InventoryItem GetEquipWeapon(WeaponType type)
        {
            return Repository.FindByType<IWeaponItemData>(
                x => x.Type == type,
                x => x.equip);
        }

        public void Equip(int index)
        {
            var inventoryItem = Repository.Get(index);
            var classType = inventoryItem.Item.GetType();
            foreach (var service in _itemServices.Where(service => service.Key.IsAssignableFrom(classType))) {
                service.Value.EquipByIndex(index, !inventoryItem.equip);
            }
        }

        public void DequipWeaponItem(HolderType holderType)
        {
            var holder = holders.Find(x => x.holderType == holderType);
            if (holder == null) return;
            if (!holder.current) return;
            var weapon = holder.current.GetComponent<IWeaponInterface>();
            weapon?.Disable(true);

            Destroy(holder.current.gameObject);
        }

        public void InitWeaponItem(HolderType holderType, IWeaponItemData itemData, bool autoShow = true)
        {
            var holder = holders.Find(x => x.holderType == holderType);
            if (holder == null) return;
            DequipWeaponItem(holderType);

            var weapon = Instantiate(itemData.Prefab);
            weapon.name = "weapon";
            holder.current = weapon.transform;
            holder.current.parent = holder.holder;

            weapon.transform.localPosition = itemData.Position;
            weapon.transform.localRotation = Quaternion.Euler(itemData.Rotation);
            
            var weaponItem = weapon.GetComponent<IWeaponInterface>();
            if (autoShow) {
                weaponItem?.Enable();
            } else {
                weaponItem?.Disable();
            }
        }

        public void SetWeaponVisible(WeaponType type, bool visible)
        {
            var weapon = _repository.FindByType<IWeaponItemData>(x => x.Type == type);
            if (weapon == null) return;
            var holderType = ((IWeaponItemData) weapon.Item).HolderType;
            var holder = holders.Find(x => x.holderType == holderType);
            if (holder == null) return;
            if (holder.current) {
                var weaponInterface = holder.current.GetComponent<IWeaponInterface>();
                if (visible) {
                    if (!holder.current.gameObject.activeSelf) {
                        holder.current.gameObject.SetActive(true);
                    }

                    weaponInterface?.Enable();
                }
                else {
                    weaponInterface?.Disable();
                }
            }
        }

        private void OnShowEvent(bool value)
        {
            if (value) {
                inventoryUi.Show();
            }
        }

        private void OnEnable()
        {
            _gameMenuManager.OnOpenEvent.AddListener(OnShowEvent);
        }

        private void OnDisable()
        {
            _gameMenuManager.OnOpenEvent.RemoveListener(OnShowEvent);
        }

        public override void Show()
        {
            inventoryUi.ShowGameObject(true);
        }

        public override void Hide()
        {
            inventoryUi.ShowGameObject(false);
        }
    }

    public interface IInventoryManager
    {
        IInventoryRepository Repository { get; }
        InventoryUI InventoryUi { get; }

        bool HasEquipWeapon(WeaponType type);
        InventoryItem GetEquipWeapon(WeaponType type);
        void Equip(int index);
        void DequipWeaponItem(HolderType holderType);
        void InitWeaponItem(HolderType holderType, IWeaponItemData itemData, bool autoShow = true);
        void SetWeaponVisible(WeaponType type, bool visible);
    }

    [System.Serializable]
    public class Holder
    {
        public HolderType holderType = HolderType.LeftArm;
        public Transform holder;
        public Transform current;
    }
}